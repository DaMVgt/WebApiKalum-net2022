using WebApiKalum.Dtos;

namespace WebApiKalum.Utilities
{
    public class HttpResponsePaginacion<T> : PaginacionDTO<T>
    {
        public HttpResponsePaginacion(IQueryable<T> source, int number)
        {
            this.Number = number;
            int cantidadRegistroPorPagina = 5;
            int totalRegistro = source.Count();
            this.TotalPages = (int)Math.Ceiling((double)totalRegistro / cantidadRegistroPorPagina);
            this.Content = source.Skip(cantidadRegistroPorPagina * Number).Take(cantidadRegistroPorPagina).ToList();
            if (this.Number == 0)
            {
                this.First = true;
            }
            else if ((this.Number + 1) == this.TotalPages)
            {
                this.Last = true;
            }
        }
    }
}