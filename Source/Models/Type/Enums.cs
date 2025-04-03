namespace OMS.Models
{
    /// <summary>
    /// Enum Printed Flag
    /// </summary>
    public enum PrintedFlag
    {
        NotPrinted = 0,
        Printed
    }

    /// <summary>
    /// Enum Finished Flag
    /// </summary>
    public enum FinishedFlag
    {
        NotFinish = 0,
        Finished
    }

    /// <summary>
    /// Enum Delete Flag
    /// </summary>
    public enum DeleteFlag
    {
        NotDelete = 0,
        Deleted
    }

    /// <summary>
    /// Enum Issued Flag
    /// </summary>
    public enum IssuedFlag
    {
        Unissued = 0,
        Issued
    }

    /// <summary>
    /// Enum Referent status
    /// </summary>
    public enum Referent
    {
        NotRef = 0,
        IncompleteRef,
        CompletedRef
    }

    /// <summary>
    /// Enum Status Flag
    /// </summary>
    public enum StatusFlag
    {
        /// <summary>
        /// Accept
        /// </summary>
        Sales = 0,

        /// <summary>
        /// A
        /// </summary>
        A,

        /// <summary>
        /// B 
        /// </summary>
        B,

        /// <summary>
        /// C 
        /// </summary>
        C,

        /// <summary>
        /// D 
        /// </summary>
        D,
        /// <summary>
        /// Failed
        /// </summary>
        Lost
    }

    /// <summary>
    /// Sort Direc
    /// </summary>
    public enum SortDirec
    {
        None = 0,
        ASC,
        DESC
    }
}
