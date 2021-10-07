namespace VivaVictoria.Chaos.Sql.Enums
{
    /*
     * Default - use TransactionMode specified in ISettings
     * None - dont use transaction
     * One - run whole migration in single transaction
     */
    public enum TransactionMode
    {
        Default,
        None,
        One
    }
}