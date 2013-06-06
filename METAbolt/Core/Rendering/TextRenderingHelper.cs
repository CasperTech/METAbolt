using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenMetaverse;
using OpenMetaverse.Rendering;
using System.Text;
using System.IO;
using System.Globalization;

namespace METAbolt
{
    public static class MeshToOBJ
    {
        public static bool MeshesToOBJ(Dictionary<uint, FacetedMesh> meshes, string filename)
        {
            StringBuilder obj = new StringBuilder();
            StringBuilder mtl = new StringBuilder();

            FileInfo objFileInfo = new FileInfo(filename);

            string mtlFilename = objFileInfo.FullName.Substring(objFileInfo.DirectoryName.Length + 1,
                objFileInfo.FullName.Length - (objFileInfo.DirectoryName.Length + 1) - 4) + ".mtl";

            obj.AppendLine("# Created by libprimrender");
            obj.AppendLine("mtllib ./" + mtlFilename);
            obj.AppendLine();

            mtl.AppendLine("# Created by libprimrender");
            mtl.AppendLine();

            int primNr = 0;
            foreach (FacetedMesh mesh in meshes.Values)
            {
                for (int j = 0; j < mesh.Faces.Count; j++)
                {
                    Face face = mesh.Faces[j];

                    if (face.Vertices.Count > 2)
                    {
                        string mtlName = String.Format(CultureInfo.CurrentCulture, "material{0}-{1}", primNr, face.ID);
                        Primitive.TextureEntryFace tex = face.TextureFace;
                        string texName = tex.TextureID.ToString() + ".tga";

                        // FIXME: Convert the source to TGA (if needed) and copy to the destination

                        float shiny = 0.00f;
                        switch (tex.Shiny)
                        {
                            case Shininess.High:
                                shiny = 1.00f;
                                break;
                            case Shininess.Medium:
                                shiny = 0.66f;
                                break;
                            case Shininess.Low:
                                shiny = 0.33f;
                                break;
                        }

                        obj.AppendFormat("g face{0}-{1}{2}", primNr, face.ID, Environment.NewLine);

                        mtl.AppendLine("newmtl " + mtlName);
                        mtl.AppendFormat(CultureInfo.CurrentCulture, "Ka {0} {1} {2}{3}", tex.RGBA.R, tex.RGBA.G, tex.RGBA.B, Environment.NewLine);
                        mtl.AppendFormat(CultureInfo.CurrentCulture, "Kd {0} {1} {2}{3}", tex.RGBA.R, tex.RGBA.G, tex.RGBA.B, Environment.NewLine);
                        //mtl.AppendFormat("Ks {0} {1} {2}{3}");
                        mtl.AppendLine("Tr " + tex.RGBA.A);
                        mtl.AppendLine("Ns " + shiny);
                        mtl.AppendLine("illum 1");
                        if (tex.TextureID != UUID.Zero && tex.TextureID != Primitive.TextureEntry.WHITE_TEXTURE)
                            mtl.AppendLine("map_Kd ./" + texName);
                        mtl.AppendLine();

                        // Write the vertices, texture coordinates, and vertex normals for this side
                        for (int k = 0; k < face.Vertices.Count; k++)
                        {
                            Vertex vertex = face.Vertices[k];

                            #region Vertex

                            OpenMetaverse.Vector3 pos = new OpenMetaverse.Vector3(vertex.Position);

                            // Apply scaling
                            pos *= mesh.Prim.Scale;

                            // Apply rotation
                            pos *= mesh.Prim.Rotation;

                            // The root prim position is sim-relative, while child prim positions are
                            // parent-relative. We want to apply parent-relative translations but not
                            // sim-relative ones
                            if (mesh.Prim.ParentID != 0)
                                pos += mesh.Prim.Position;

                            obj.AppendFormat(CultureInfo.CurrentCulture, "v {0} {1} {2}{3}", pos.X, pos.Y, pos.Z, Environment.NewLine);

                            #endregion Vertex

                            #region Texture Coord

                            obj.AppendFormat("vt {0} {1}{2}", vertex.TexCoord.X, vertex.TexCoord.Y,
                                Environment.NewLine);

                            #endregion Texture Coord

                            #region Vertex Normal

                            // HACK: Sometimes normals are getting set to <NaN,NaN,NaN>
                            if (!Single.IsNaN(vertex.Normal.X) && !Single.IsNaN(vertex.Normal.Y) && !Single.IsNaN(vertex.Normal.Z))
                                obj.AppendFormat(CultureInfo.CurrentCulture, "vn {0} {1} {2}{3}", vertex.Normal.X, vertex.Normal.Y, vertex.Normal.Z,
                                    Environment.NewLine);
                            else
                                obj.AppendLine("vn 0.0 1.0 0.0");

                            #endregion Vertex Normal
                        }

                        obj.AppendFormat(CultureInfo.CurrentCulture, "# {0} vertices{1}", face.Vertices.Count, Environment.NewLine);
                        obj.AppendLine();
                        obj.AppendLine("usemtl " + mtlName);

                        #region Elements

                        // Write all of the faces (triangles) for this side
                        for (int k = 0; k < face.Indices.Count / 3; k++)
                        {
                            obj.AppendFormat(CultureInfo.CurrentCulture, "f -{0}/-{0}/-{0} -{1}/-{1}/-{1} -{2}/-{2}/-{2}{3}",
                                face.Vertices.Count - face.Indices[k * 3 + 0],
                                face.Vertices.Count - face.Indices[k * 3 + 1],
                                face.Vertices.Count - face.Indices[k * 3 + 2],
                                Environment.NewLine);
                        }

                        obj.AppendFormat("# {0} elements{1}", face.Indices.Count / 3, Environment.NewLine);
                        obj.AppendLine();

                        #endregion Elements
                    }
                }
                primNr++;
            }

            try
            {
                File.WriteAllText(filename, obj.ToString());
                File.WriteAllText(mtlFilename, mtl.ToString());
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }

    public static class Math3D
    {
        // Column-major:
        // |  0  4  8 12 |
        // |  1  5  9 13 |
        // |  2  6 10 14 |
        // |  3  7 11 15 |

        public static float[] CreateTranslationMatrix(OpenMetaverse.Vector3 v)
        {
            float[] mat = new float[16];

            mat[12] = v.X;
            mat[13] = v.Y;
            mat[14] = v.Z;
            mat[0] = mat[5] = mat[10] = mat[15] = 1;

            return mat;
        }

        public static float[] CreateRotationMatrix(OpenMetaverse.Quaternion q)
        {
            float[] mat = new float[16];

            // Transpose the quaternion (don't ask me why)
            q.X = q.X * -1f;
            q.Y = q.Y * -1f;
            q.Z = q.Z * -1f;

            float x2 = q.X + q.X;
            float y2 = q.Y + q.Y;
            float z2 = q.Z + q.Z;
            float xx = q.X * x2;
            float xy = q.X * y2;
            float xz = q.X * z2;
            float yy = q.Y * y2;
            float yz = q.Y * z2;
            float zz = q.Z * z2;
            float wx = q.W * x2;
            float wy = q.W * y2;
            float wz = q.W * z2;

            mat[0] = 1.0f - (yy + zz);
            mat[1] = xy - wz;
            mat[2] = xz + wy;
            mat[3] = 0.0f;

            mat[4] = xy + wz;
            mat[5] = 1.0f - (xx + zz);
            mat[6] = yz - wx;
            mat[7] = 0.0f;

            mat[8] = xz - wy;
            mat[9] = yz + wx;
            mat[10] = 1.0f - (xx + yy);
            mat[11] = 0.0f;

            mat[12] = 0.0f;
            mat[13] = 0.0f;
            mat[14] = 0.0f;
            mat[15] = 1.0f;

            return mat;
        }

        public static float[] CreateScaleMatrix(OpenMetaverse.Vector3 v)
        {
            float[] mat = new float[16];

            mat[0] = v.X;
            mat[5] = v.Y;
            mat[10] = v.Z;
            mat[15] = 1;

            return mat;
        }

        public static bool GluProject(OpenTK.Vector3 objPos, OpenTK.Matrix4 modelMatrix, OpenTK.Matrix4 projMatrix, int[] viewport, out OpenTK.Vector3 screenPos)
        {
            OpenTK.Vector4 _in;
            OpenTK.Vector4 _out;

            _in.X = objPos.X;
            _in.Y = objPos.Y;
            _in.Z = objPos.Z;
            _in.W = 1.0f;

            _out = OpenTK.Vector4.Transform(_in, modelMatrix);
            _in = OpenTK.Vector4.Transform(_out, projMatrix);

            if (_in.W <= 0.0)
            {
                screenPos = OpenTK.Vector3.Zero;
                return false;
            }

            _in.X /= _in.W;
            _in.Y /= _in.W;
            _in.Z /= _in.W;
            /* Map x, y and z to range 0-1 */
            _in.X = _in.X * 0.5f + 0.5f;
            _in.Y = _in.Y * 0.5f + 0.5f;
            _in.Z = _in.Z * 0.5f + 0.5f;

            /* Map x,y to viewport */
            _in.X = _in.X * viewport[2] + viewport[0];
            _in.Y = _in.Y * viewport[3] + viewport[1];

            screenPos.X = _in.X;
            screenPos.Y = _in.Y;
            screenPos.Z = _in.Z;

            return true;
        }

        public static bool GluUnProject(float winx, float winy, float winz, OpenTK.Matrix4 modelMatrix, OpenTK.Matrix4 projMatrix, int[] viewport, out OpenTK.Vector3 pos)
        {
            OpenTK.Matrix4 finalMatrix;
            OpenTK.Vector4 _in;
            OpenTK.Vector4 _out;

            finalMatrix = OpenTK.Matrix4.Mult(modelMatrix, projMatrix);

            finalMatrix.Invert();

            _in.X = winx;
            _in.Y = winy;
            _in.Z = winz;
            _in.W = 1.0f;

            /* Map x and y from window coordinates */
            _in.X = (_in.X - viewport[0]) / viewport[2];
            _in.Y = (_in.Y - viewport[1]) / viewport[3];

            pos = OpenTK.Vector3.Zero;

            /* Map to range -1 to 1 */
            _in.X = _in.X * 2 - 1;
            _in.Y = _in.Y * 2 - 1;
            _in.Z = _in.Z * 2 - 1;

            //__gluMultMatrixVecd(finalMatrix, _in, _out);
            // check if this works:
            _out = OpenTK.Vector4.Transform(_in, finalMatrix);

            if (_out.W == 0.0f)
                return false;
            _out.X /= _out.W;
            _out.Y /= _out.W;
            _out.Z /= _out.W;
            pos.X = _out.X;
            pos.Y = _out.Y;
            pos.Z = _out.Z;

            return true;
        }
    }

    public class FaceData
    {
        public float[] Vertices;
        public ushort[] Indices;
        public float[] TexCoords;
        public float[] Normals;
        public int TexturePointer;
        public int PickingID = -1;
        public System.Drawing.Image Texture;
        public bool IsAlpha;
    }

    public class TextureLoadItem
    {
        public FaceData Data;
        public Primitive Prim;
        public Primitive.TextureEntryFace TeFace;
    }

    public static class Render
    {
        public static IRendering Plugin;
    }
}
