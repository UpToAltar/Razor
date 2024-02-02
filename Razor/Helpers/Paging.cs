namespace Razor.Helpers;

public class Paging
{
    public int currentPage {set; get;}
    public int countPages {set; get;}
    public Func<int?, string> generateUrl {set; get;}
}