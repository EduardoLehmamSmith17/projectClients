using Moq;
using projeto_clientes.Data;
using projeto_clientes.Models;
using projeto_clientes.Repositorio;

namespace TesteProjetoPessoas
{
    public class testePessoaJuridica
    {
        [Fact]
        public void Test_Add_PessoaJuridica()
        {
            // Arrange
            var mockContext = new Mock<dbContext>();
            var repository = new PessoaJuridicaRepositorio(mockContext.Object);
            var pessoaJuridica = new PessoaJuridica
            {
                CNPJ = "12345678000195",
                RazaoSocial = "Empresa Teste LTDA",
                NomeFantasia = "Empresa Teste",
                Endereco = "Av. Teste, 789"
            };

            // Act
            repository.Add(pessoaJuridica);

            // Assert
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
            mockContext.Verify(c => c.PessoasJuridicas.Add(It.IsAny<PessoaJuridica>()), Times.Once);
        }

        [Fact]
        public void Test_Update_PessoaJuridica_Existing()
        {
            // Arrange
            var mockContext = new Mock<dbContext>();
            var repository = new PessoaJuridicaRepositorio(mockContext.Object);
            var pessoaJuridica = new PessoaJuridica
            {
                CNPJ = "12345678000195",
                RazaoSocial = "Nova Razão Social",
                NomeFantasia = "Novo Nome Fantasia",
                Endereco = "Nova Av. Teste, 987"
            };

            mockContext.Setup(c => c.PessoasJuridicas.FirstOrDefault(p => p.CNPJ == pessoaJuridica.CNPJ))
                       .Returns(new PessoaJuridica { CNPJ = pessoaJuridica.CNPJ });

            // Act
            repository.Update(pessoaJuridica, pessoaJuridica.CNPJ);

            // Assert
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Test_Update_PessoaJuridica_NotFound()
        {
            // Arrange
            var mockContext = new Mock<dbContext>();
            var repository = new PessoaJuridicaRepositorio(mockContext.Object);
            var pessoaJuridica = new PessoaJuridica
            {
                CNPJ = "99999999000199",
                RazaoSocial = "Nova Razão Social",
                NomeFantasia = "Novo Nome Fantasia",
                Endereco = "Nova Av. Teste, 987"
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Update(pessoaJuridica, pessoaJuridica.CNPJ));
        }

        [Fact]
        public void Test_Delete_PessoaJuridica_Existing()
        {
            // Arrange
            var mockContext = new Mock<dbContext>();
            var repository = new PessoaJuridicaRepositorio(mockContext.Object);
            var cnpjToDelete = "12345678000195";

            mockContext.Setup(c => c.PessoasJuridicas.FirstOrDefault(p => p.CNPJ == cnpjToDelete))
                       .Returns(new PessoaJuridica { CNPJ = cnpjToDelete });

            // Act
            repository.Delete(cnpjToDelete);

            // Assert
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Test_Delete_PessoaJuridica_NotFound()
        {
            // Arrange
            var mockContext = new Mock<dbContext>();
            var repository = new PessoaJuridicaRepositorio(mockContext.Object);
            var cnpjToDelete = "99999999000199";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Delete(cnpjToDelete));
        }

        [Fact]
        public void Test_Get_PessoaJuridica_ByCNPJ()
        {
            // Arrange
            var mockContext = new Mock<dbContext>();
            var repository = new PessoaJuridicaRepositorio(mockContext.Object);
            var cnpjToSearch = "12345678000195";
            var mockData = new List<PessoaJuridica>
            {
                new PessoaJuridica { CNPJ = cnpjToSearch, RazaoSocial = "Empresa Teste", NomeFantasia = "Empresa Teste LTDA", Endereco = "Av. Teste, 789" }
            }.AsQueryable();

            mockContext.Setup(c => c.PessoasJuridicas).Returns((Microsoft.EntityFrameworkCore.DbSet<PessoaJuridica>)MockDbSet(mockData));

            // Act
            var result = repository.Get(cnpjToSearch);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(cnpjToSearch, result.First().CNPJ);
        }

        [Fact]
        public void Test_Get_PessoaJuridica_All()
        {
            // Arrange
            var mockContext = new Mock<dbContext>();
            var repository = new PessoaJuridicaRepositorio(mockContext.Object);
            var mockData = new List<PessoaJuridica>
            {
                new PessoaJuridica { CNPJ = "11111111000111", RazaoSocial = "Empresa 1", NomeFantasia = "Empresa 1 LTDA", Endereco = "Av. 1, 123" },
                new PessoaJuridica { CNPJ = "22222222000222", RazaoSocial = "Empresa 2", NomeFantasia = "Empresa 2 LTDA", Endereco = "Av. 2, 456" }
            }.AsQueryable();

            mockContext.Setup(c => c.PessoasJuridicas).Returns((Microsoft.EntityFrameworkCore.DbSet<PessoaJuridica>)MockDbSet(mockData));

            // Act
            var result = repository.Get(null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        private static IQueryable<T> MockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var mockSet = new Mock<IQueryable<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet.Object;
        }
    }
}
