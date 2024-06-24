using Microsoft.EntityFrameworkCore;
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
            var options = new DbContextOptionsBuilder<dbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Add_PJ")
                .Options;
            var context = new dbContext(options);

            var repository = new PessoaJuridicaRepositorio(context);
            var pessoaJuridica = new PessoaJuridica
            {
                CNPJ = "12345678000199",
                RazaoSocial = "Empresa Teste",
                NomeFantasia = "Teste",
                Endereco = "Rua Teste, 123",
                Contatos = new List<Contato>
            {
                new Contato { Email = "contato@empresa.com", Telefone = "123456789" }
            }
            };

            // Act
            repository.Add(pessoaJuridica);

            // Assert
            var savedPessoaJuridica = context.PessoasJuridicas.FirstOrDefault(p => p.CNPJ == "12345678000199");
            Assert.NotNull(savedPessoaJuridica);
            Assert.Equal("Empresa Teste", savedPessoaJuridica.RazaoSocial);
            Assert.Equal("Teste", savedPessoaJuridica.NomeFantasia);
            Assert.Equal("Rua Teste, 123", savedPessoaJuridica.Endereco);
            Assert.Single(savedPessoaJuridica.Contatos!);
        }

        [Fact]
        public void Test_Update_PessoaJuridica_Existing()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<dbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Update_PJ")
                .Options;

            using (var context = new dbContext(options))
            {
                var repository = new PessoaJuridicaRepositorio(context);

                var pessoaJuridicaInicial = new PessoaJuridica
                {
                    CNPJ = "12345678000199",
                    RazaoSocial = "Empresa Teste",
                    NomeFantasia = "Teste",
                    Endereco = "Rua Teste, 123",
                    Contatos = new List<Contato>
                {
                    new Contato { Email = "novo@empresa.com", Telefone = "987654321" }
                }
                };

                context.PessoasJuridicas.Add(pessoaJuridicaInicial);
                context.SaveChanges();

                var pessoaJuridicaAtualizada = new PessoaJuridica
                {
                    CNPJ = "12345678000199",
                    RazaoSocial = "Empresa Atualizada",
                    NomeFantasia = "Atualizada",
                    Endereco = "Nova Rua, 456",
                    Contatos = new List<Contato>
                {
                    new Contato { Email = "novo@empresa.com", Telefone = "987654321" }
                }
                };

                // Act
                repository.Update(pessoaJuridicaAtualizada, pessoaJuridicaAtualizada.CNPJ);

                // Assert
                var updatedPessoaJuridica = context.PessoasJuridicas.FirstOrDefault(p => p.CNPJ == "12345678000199");
                Assert.NotNull(updatedPessoaJuridica);
                Assert.Equal("Empresa Atualizada", updatedPessoaJuridica.RazaoSocial);
                Assert.Equal("Atualizada", updatedPessoaJuridica.NomeFantasia);
                Assert.Equal("Nova Rua, 456", updatedPessoaJuridica.Endereco);
                Assert.Single(updatedPessoaJuridica.Contatos!);
                Assert.Equal("novo@empresa.com", updatedPessoaJuridica.Contatos!.First().Email);
                Assert.Equal("987654321", updatedPessoaJuridica.Contatos!.First().Telefone);
            }
        }

        [Fact]
        public void Test_Update_PessoaJuridica_NotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<dbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Update_PJ_NotFound")
                .Options;
            var context = new dbContext(options);
            var repository = new PessoaJuridicaRepositorio(context);
            var pessoaJuridica = new PessoaJuridica
            {
                CNPJ = "99999999000199",
                RazaoSocial = "Nova Razão Social",
                NomeFantasia = "Novo Nome Fantasia",
                Endereco = "Nova Av. Teste, 987",
                Contatos = new List<Contato>
        {
            new Contato { Email = "novo@empresa.com", Telefone = "987654321" }
        }
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => repository.Update(pessoaJuridica, pessoaJuridica.CNPJ));
            Assert.Equal("Pessoa jurídica não encontrada para o CNPJ especificado. (Parameter 'cnpj')", ex.Message);
        }

        [Fact]
        public void Test_Delete_PessoaJuridica_Existing()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<dbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Delete_PJ_Existing")
                .Options;
            var context = new dbContext(options);
            var repository = new PessoaJuridicaRepositorio(context);
            var cnpjToDelete = "12345678000195";

            var pessoaJuridica = new PessoaJuridica
            {
                CNPJ = cnpjToDelete,
                RazaoSocial = "Empresa Teste",
                NomeFantasia = "Empresa Teste LTDA",
                Endereco = "Av. Teste, 789",
                Contatos = new List<Contato>
        {
            new Contato { Email = "contato@empresa.com", Telefone = "123456789" }
        }
            };
            context.PessoasJuridicas.Add(pessoaJuridica);
            context.SaveChanges();

            // Act
            repository.Delete(cnpjToDelete);

            // Assert
            var deletedPessoaJuridica = context.PessoasJuridicas.FirstOrDefault(p => p.CNPJ == cnpjToDelete);
            Assert.Null(deletedPessoaJuridica);
        }

        [Fact]
        public void Test_Delete_PessoaJuridica_NotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<dbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Delete_PJ_NotFound")
                .Options;
            var context = new dbContext(options);
            var repository = new PessoaJuridicaRepositorio(context);
            var cnpjToDelete = "99999999000199";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => repository.Delete(cnpjToDelete));
            Assert.Equal("Pessoa jurídica não encontrada para o CNPJ especificado. (Parameter 'cnpj')", ex.Message);
        }

        [Fact]
        public void Test_Get_PessoaJuridica_ByCNPJ()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<dbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Get_PJ_ByCNPJ")
                .Options;
            var context = new dbContext(options);
            var repository = new PessoaJuridicaRepositorio(context);
            var cnpjToSearch = "12345678000195";

            var mockData = new List<PessoaJuridica>
        {
            new PessoaJuridica
            {
                CNPJ = cnpjToSearch,
                RazaoSocial = "Empresa Teste",
                NomeFantasia = "Empresa Teste LTDA",
                Endereco = "Av. Teste, 789",
                Contatos = new List<Contato>
                {
                    new Contato { Email = "contato@empresa.com", Telefone = "123456789" }
                }
            }
        };

            context.PessoasJuridicas.AddRange(mockData);
            context.SaveChanges();

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
            var options = new DbContextOptionsBuilder<dbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Get_PJ_All")
                .Options;
            var context = new dbContext(options);
            var repository = new PessoaJuridicaRepositorio(context);

            var mockData = new List<PessoaJuridica>
        {
            new PessoaJuridica
            {
                CNPJ = "11111111000111",
                RazaoSocial = "Empresa 1",
                NomeFantasia = "Empresa 1 LTDA",
                Endereco = "Av. 1, 123",
                Contatos = new List<Contato>
                {
                    new Contato { Email = "contato1@empresa1.com", Telefone = "111111111" }
                }
            },
            new PessoaJuridica
            {
                CNPJ = "22222222000222",
                RazaoSocial = "Empresa 2",
                NomeFantasia = "Empresa 2 LTDA",
                Endereco = "Av. 2, 456",
                Contatos = new List<Contato>
                {
                    new Contato { Email = "contato2@empresa2.com", Telefone = "222222222" }
                }
            }
        };

            context.PessoasJuridicas.AddRange(mockData);
            context.SaveChanges();

            // Act
            var result = repository.Get(null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
