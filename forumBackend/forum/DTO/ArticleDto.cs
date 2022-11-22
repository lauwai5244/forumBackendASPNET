namespace forum.DTO
{
    public class ArticleDto
    {

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        

      
    }

    public class GetArticleListDto
    {

        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;


        public Guid CreationUserId { get; set; }




    }

    public class UpdateArticleListDto
    {

        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;





    }
}


