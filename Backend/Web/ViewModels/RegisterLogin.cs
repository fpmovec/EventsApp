using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    internal record LoginViewModel(string login, string password);

    internal record RegidterViewModel(string login, string password, [EmailAddress]string email); 
}
