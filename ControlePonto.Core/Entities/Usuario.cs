using ControlePonto.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Entities
{
    public class Usuario : EntityBase, IEquatable<Usuario>, IEqualityComparer<Usuario>
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CentroCusto { get; set; }
        public TimeSpan CargaHoraria { get; set; }
        public TimeSpan CargaHorariaSexta { get; set; }
        public List<Ponto> Pontos { get; set; }
        public TimeSpan Saldo { get; set; }

        public bool Equals(Usuario other)
        {
            if (other is null) return false;

            return
                this.Nome == other.Nome &&
                this.Email == other.Email &&
                this.CentroCusto == other.CentroCusto; 
                //this.CargaHoraria == other.CargaHoraria &&
                //this.CargaHorariaSexta == other.CargaHoraria &&
                //this.Saldo == other.Saldo;
        }

        internal void RegistrarPonto(List<Ponto> pontos)
        {
            Pontos = pontos;
            foreach (var ponto in pontos)
            {
                Saldo += ponto.Saldo;
            }
        }

        public override bool Equals(object obj) => Equals(obj as Usuario);
        public override int GetHashCode() => (Nome, Email, CentroCusto, CargaHoraria, CargaHorariaSexta, Saldo).GetHashCode();

        public bool Equals(Usuario x, Usuario y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.Nome == y.Nome && x.Email == y.Email && x.CentroCusto == y.CentroCusto;
        }

        public int GetHashCode(Usuario usuario)
        {
            // Check whether the object is null
            if (Object.ReferenceEquals(usuario, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashProductName = usuario.Nome == null ? 0 : usuario.Nome.GetHashCode();

            //Get hash code for the Code field.
            //int hashProductCode = usuario.Code.GetHashCode();

            //Calculate the hash code for the product.
            return hashProductName;
        }
    }
}
