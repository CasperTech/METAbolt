using System;
using System.Windows.Forms;

namespace METAbolt
{
    #region ProcessNodeEventArgs

    /// <summary>
    /// The event arguments used by the ProcessNode event of TreeViewWalker.
    /// </summary>
    public class ProcessNodeEventArgs : EventArgs
    {
        #region Data

        private static ProcessNodeEventArgs instance;

        private TreeNode node;
        private bool processDescendants;
        private bool processSiblings;
        private bool stopProcessing;

        #endregion // Data

        #region Private Constructor

        private ProcessNodeEventArgs()
        {
        }

        #endregion // Private Constructor

        #region CreateInstance

        /// <summary>
        /// This method ensures that the ProcessNodeEventArgs class is a singleton.  Making this class
        /// a singleton prevents multiple instances from being created, which could help to prevent
        /// heap fragmentation when the tree being navigated has hundreds or thousands of nodes.
        /// </summary>
        /// <param name="node">The node to be exposed by the event argument.</param>
        internal static ProcessNodeEventArgs CreateInstance(TreeNode node)
        {
            if (ProcessNodeEventArgs.instance == null)
                ProcessNodeEventArgs.instance = new ProcessNodeEventArgs();

            ProcessNodeEventArgs.instance.node = node;
            ProcessNodeEventArgs.instance.processDescendants = true;
            ProcessNodeEventArgs.instance.processSiblings = true;
            ProcessNodeEventArgs.instance.stopProcessing = false;

            return ProcessNodeEventArgs.instance;
        }

        #endregion // CreateInstance

        #region Public Interface

        #region Node

        /// <summary>
        /// Returns the TreeNode to process.
        /// </summary>
        public TreeNode Node
        {
            get { return this.node; }
        }

        #endregion // Node

        #region ProcessDescendants

        /// <summary>
        /// Gets/sets a value which determines whether the ProcessNode event should be raised for the descendant 
        /// nodes of the current TreeNode.  The default value is true.  If StopProcessing is set to true, this 
        /// property is ignored.
        /// </summary>
        public bool ProcessDescendants
        {
            get { return this.processDescendants; }
            set { this.processDescendants = value; }
        }

        #endregion // ProcessDescendants

        #region ProcessSiblings

        /// <summary>
        /// Gets/sets a value which determines whether the ProcessNode event should be raised for the unprocessed sibling 
        /// nodes of the current TreeNode.  The default value is true.  If StopProcessing is set to true, this 
        /// property is ignored.
        /// </summary>
        public bool ProcessSiblings
        {
            get { return this.processSiblings; }
            set { this.processSiblings = value; }
        }

        #endregion // ProcessSiblings

        #region StopProcessing

        /// <summary>
        /// Gets/sets whether the ProcessNode event should be raised for any of the remaining nodes in the TreeView.
        /// If this property is set to true, the ProcessDescendants and ProcessSiblings properties are ignored.
        /// </summary>
        public bool StopProcessing
        {
            get { return this.stopProcessing; }
            set { this.stopProcessing = value; }
        }

        #endregion // StopProcessing

        #endregion // Public Interface
    }

    #endregion // ProcessNodeEventArgs

    #region ProcessNodeEventHandler

    /// <summary>
    /// The delegate used by the ProcessNode event of TreeViewWalker.
    /// </summary>
    public delegate void ProcessNodeEventHandler(object sender, ProcessNodeEventArgs e);

    #endregion // ProcessNodeEventHandler
}