namespace texo.data.Dtos
{
    /// <summary>
    /// Row from CSV file
    /// year;title;studios;producers;winner
    /// </summary>
    /// <example>
    /// 1980;Can't Stop the Music;Associated Film Distribution;Allan Carr;yes
    /// 1980;Cruising;Lorimar Productions, United Artists;Jerry Weintraub;
    /// </example>
    public class CsvMovieDto
    {
        public int Year { get; set; }
        public string Title { get; set; }
        public string Studios { get; set; }
        public string Producers { get; set; }
        public string Winner { get; set; }
    }
}