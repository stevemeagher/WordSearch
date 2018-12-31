namespace WordSearch.WordSearchLib
{
    public interface IGridManager
    {
        string[,] Grid {get;}
        bool IsGridValidated {get;}
        void ValidateGrid();
    }
}