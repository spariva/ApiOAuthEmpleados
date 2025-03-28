using ApiOAuthEmpleados.Data;
using ApiOAuthEmpleados.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiOAuthEmpleados.Repositories
{
    public class RepositoryEmpleados
    {
        private HospitalContext context;
        public RepositoryEmpleados(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> FindEmpleadoAsync(int id)
        {
            return await this.context.Empleados.FindAsync(id);
            //return await this.context.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == id);
            //            1.Tracking: FindAsync checks the context for already tracked entities, while FirstOrDefaultAsync does not.
            //2.Query Flexibility: FindAsync is limited to primary key lookups, whereas FirstOrDefaultAsync can be used with any condition.
            //3.Performance: FindAsync can be more efficient for primary key lookups because it avoids a database query if the entity is already tracked.
        }

        public async Task<List<Empleado>> GetCompisEmpleadoAsync(int idDepartamento)
        {
            return await this.context.Empleados
            .Where(x => x.IdDepartamento == idDepartamento)
            .ToListAsync();
        }

        public async Task<Empleado> LoginAsync(string apellido, int idEmpleado)
        {
            return await this.context.Empleados.FirstOrDefaultAsync(x => x.Apellido == apellido && x.IdEmpleado == idEmpleado);
            //return await this.context.Empleados.Where(x => x.Apellido == apellido && x.IdEmpleado == idEmpleado).FirstOrDefaultAsync();
//            1.Conciseness: The FirstOrDefaultAsync with predicate is more concise and directly expresses the intent to find the first matching entity.
//2.Flexibility: Using Where followed by FirstOrDefaultAsync can be more flexible if you need to apply additional query operations before finding the first entity.
//3.Performance: Both methods generate similar SQL queries and have similar performance characteristics.The choice between them is more about code readability and flexibility.
        }
    }
}
