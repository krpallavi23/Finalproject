using System;
using System.Web.UI;

public partial class About : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // This method runs every time the page is loaded
        if (!IsPostBack)
        {
            // You can put your initialization logic here
            InitializePage();
        }
    }

    private void InitializePage()
    {
        // You can add any logic to initialize the page,
        // such as fetching data from a database or setting properties.
        // For example, you could set a title or load team member information here.
    }
}
