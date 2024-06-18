using Moq;
using projeto_clientes.Data;
using projeto_clientes.Models;
using projeto_clientes.Repositorio;

namespace TesteProjetoPessoas
{
    public class testePessoaFisica
    {
        public class PessoaFisicaRepositorioTests
        {
            [Fact]
            public void Test_Add_PessoaFisica()
            {
                // Arrange
                var mockContext = new Mock<dbContext>();
                var repository = new PessoaFisicaRepositorio(mockContext.Object);
                var pessoaFisica = new PessoaFisica
                {
                    CPF = "12345678900",
                    NomeCompleto = "Fulano de Tal",
                    DataDeNascimento = new DateTime(1990, 1, 1),
                    Endereco = "Rua Teste, 123"
                };

                // Act
                repository.Add(pessoaFisica);

                // Assert
                mockContext.Verify(c => c.SaveChanges(), Times.Once);
                mockContext.Verify(c => c.PessoasFisicas.Add(It.IsAny<PessoaFisica>()), Times.Once);
            }

            [Fact]
            public void Test_Update_PessoaFisica_Existing()
            {
                // Arrange
                var mockContext = new Mock<dbContext>();
                var repository = new PessoaFisicaRepositorio(mockContext.Object);
                var pessoaFisica = new PessoaFisica
                {
                    CPF = "12345678900",
                    NomeCompleto = "Novo Nome",
                    DataDeNascimento = new DateTime(1990, 1, 1),
                    Endereco = "Nova Rua, 456"
                };

                mockContext.Setup(c => c.PessoasFisicas.FirstOrDefault(p => p.CPF == pessoaFisica.CPF))
                           .Returns(new PessoaFisica { CPF = pessoaFisica.CPF });

                // Act
                repository.Update(pessoaFisica, pessoaFisica.CPF);

                // Assert
                mockContext.Verify(c => c.SaveChanges(), Times.Once);
            }

            [Fact]
            public void Test_Update_PessoaFisica_NotFound()
            {
                // Arrange
                var mockContext = new Mock<dbContext>();
                var repository = new PessoaFisicaRepositorio(mockContext.Object);
                var pessoaFisica = new PessoaFisica
                {
                    CPF = "99999999999",
                    NomeCompleto = "Novo Nome",
                    DataDeNascimento = new DateTime(1990, 1, 1),
                    Endereco = "Nova Rua, 456"
                };

                // Act & Assert
                Assert.Throws<ArgumentException>(() => repository.Update(pessoaFisica, pessoaFisica.CPF));
            }

            [Fact]
            public void Test_Delete_PessoaFisica_Existing()
            {
                // Arrange
                var mockContext = new Mock<dbContext>();
                var repository = new PessoaFisicaRepositorio(mockContext.Object);
                var cpfToDelete = "12345678900";

                mockContext.Setup(c => c.PessoasFisicas.FirstOrDefault(p => p.CPF == cpfToDelete))
                           .Returns(new PessoaFisica { CPF = cpfToDelete });

                // Act
                repository.Delete(cpfToDelete);

                // Assert
                mockContext.Verify(c => c.SaveChanges(), Times.Once);
            }

            [Fact]
            public void Test_Delete_PessoaFisica_NotFound()
            {
                // Arrange
                var mockContext = new Mock<dbContext>();
                var repository = new PessoaFisicaRepositorio(mockContext.Object);
                var cpfToDelete = "99999999999";

                // Act & Assert
                Assert.Throws<ArgumentException>(() => repository.Delete(cpfToDelete));
            }

            [Fact]
            public void Test_Get_PessoaFisica_ByCPF()
            {
                // Arrange
                var mockContext = new Mock<dbContext>();
                var repository = new PessoaFisicaRepositorio(mockContext.Object);
                var cpfToSearch = "12345678900";
                var mockData = new List<PessoaFisica>
            {
                new PessoaFisica { CPF = cpfToSearch, NomeCompleto = "Teste", DataDeNascimento = DateTime.Now, Endereco = "Rua Teste" }
            }.AsQueryable();

                mockContext.Setup(c => c.PessoasFisicas).Returns((Microsoft.EntityFrameworkCore.DbSet<PessoaFisica>)MockDbSet(mockData));

                // Act
                var result = repository.Get(cpfToSearch);

                // Assert
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Equal(cpfToSearch, result.First().CPF);
            }

            [Fact]
            public void Test_Get_PessoaFisica_All()
            {
                // Arrange
                var mockContext = new Mock<dbContext>();
                var repository = new PessoaFisicaRepositorio(mockContext.Object);
                var mockData = new List<PessoaFisica>
            {
                new PessoaFisica { CPF = "11111111111", NomeCompleto = "Teste1", DataDeNascimento = DateTime.Now, Endereco = "Rua Teste1" },
                new PessoaFisica { CPF = "22222222222", NomeCompleto = "Teste2", DataDeNascimento = DateTime.Now, Endereco = "Rua Teste2" }
            }.AsQueryable();

                mockContext.Setup(c => c.PessoasFisicas).Returns((Microsoft.EntityFrameworkCore.DbSet<PessoaFisica>)MockDbSet(mockData));

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
}