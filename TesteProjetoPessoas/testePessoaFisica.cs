using Microsoft.EntityFrameworkCore;
using projeto_clientes.Data;
using projeto_clientes.Models;
using projeto_clientes.Repositorio;
using projeto_clientes.Validations;

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
                var options = new DbContextOptionsBuilder<dbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase_Add")
                    .Options;

                using (var context = new dbContext(options))
                {
                    var repository = new PessoaFisicaRepositorio(context);
                    var pessoaFisica = new PessoaFisica
                    {
                        CPF = "12345678900",
                        NomeCompleto = "Fulano de Tal",
                        DataDeNascimento = new DateTime(1990, 1, 1),
                        Endereco = "Rua Teste, 123",
                        // Contatos não fornecidos
                    };

                    // Act & Assert
                    var exception = Assert.Throws<ArgumentException>(() => repository.Add(pessoaFisica));
                    Assert.Equal("Contatos são obrigatórios.", exception.Message);

                    // Verify that no entity was saved
                    var savedPessoaFisica = context.PessoasFisicas.FirstOrDefault(p => p.CPF == "12345678900");
                    Assert.Null(savedPessoaFisica);
                }
            }

            [Fact]
            public void Test_Update_PessoaFisica_Existing()
            {
                // Arrange
                var options = new DbContextOptionsBuilder<dbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase_Update")
                    .Options;

                using (var context = new dbContext(options))
                {
                    var repository = new PessoaFisicaRepositorio(context);

                    var pessoaFisicaInicial = new PessoaFisica
                    {
                        CPF = "12345678900",
                        NomeCompleto = "Fulano de Tal",
                        DataDeNascimento = new DateTime(1990, 1, 1),
                        Endereco = "Rua Teste, 123",
                        Contatos = new List<Contato>
                {
                    new Contato { Email = "novonome@example.com", Telefone = "987654321" }
                }
                    };

                    context.PessoasFisicas.Add(pessoaFisicaInicial);
                    context.SaveChanges();

                    var pessoaFisicaAtualizada = new PessoaFisica
                    {
                        CPF = "12345678900",
                        NomeCompleto = "Novo Nome",
                        DataDeNascimento = new DateTime(1990, 1, 1),
                        Endereco = "Nova Rua, 456",
                        Contatos = new List<Contato>
                {
                    new Contato { Email = "novonome@example.com", Telefone = "987654321" }
                }
                    };

                    // Act
                    repository.Update(pessoaFisicaAtualizada, pessoaFisicaAtualizada.CPF);

                    // Assert
                    var updatedPessoaFisica = context.PessoasFisicas.FirstOrDefault(p => p.CPF == "12345678900");
                    Assert.NotNull(updatedPessoaFisica);
                    Assert.Equal("Novo Nome", updatedPessoaFisica.NomeCompleto);
                    Assert.Equal(new DateTime(1990, 1, 1), updatedPessoaFisica.DataDeNascimento);
                    Assert.Equal("Nova Rua, 456", updatedPessoaFisica.Endereco);
                    Assert.NotNull(updatedPessoaFisica.Contatos);
                    Assert.Single(updatedPessoaFisica.Contatos); // Verifica se há um contato após a atualização
                    Assert.Equal("novonome@example.com", updatedPessoaFisica.Contatos.First().Email);
                    Assert.Equal("987654321", updatedPessoaFisica.Contatos.First().Telefone);
                }
            }

            [Fact]
            public void Test_Update_PessoaFisica_NotFound()
            {
                // Arrange
                var options = new DbContextOptionsBuilder<dbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase_NotFound")
                    .Options;

                using (var context = new dbContext(options))
                {
                    var repository = new PessoaFisicaRepositorio(context);

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
            }

            [Fact]
            public void Test_Delete_PessoaFisica_Existing()
            {
                // Arrange
                var options = new DbContextOptionsBuilder<dbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase_DeleteExisting")
                    .Options;

                using (var context = new dbContext(options))
                {
                    var repository = new PessoaFisicaRepositorio(context);
                    var pessoaFisica = new PessoaFisica
                    {
                        CPF = "12345678900",
                        NomeCompleto = "Fulano de Tal",
                        DataDeNascimento = new DateTime(1990, 1, 1),
                        Endereco = "Rua Teste, 123"
                    };

                    context.PessoasFisicas.Add(pessoaFisica);
                    context.SaveChanges();

                    // Act
                    repository.Delete(pessoaFisica.CPF);

                    // Assert
                    var deletedPessoaFisica = context.PessoasFisicas.FirstOrDefault(p => p.CPF == pessoaFisica.CPF);
                    Assert.Null(deletedPessoaFisica);
                }
            }

            [Fact]
            public void Test_Delete_PessoaFisica_NotFound()
            {
                // Arrange
                var options = new DbContextOptionsBuilder<dbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase_DeleteNotFound")
                    .Options;

                using (var context = new dbContext(options))
                {
                    var repository = new PessoaFisicaRepositorio(context);
                    var cpfToDelete = "99999999999";

                    // Act & Assert
                    var exception = Assert.Throws<ArgumentException>(() => repository.Delete(cpfToDelete));
                    Assert.Equal("Pessoa física não encontrada para o CPF especificado. (Parameter 'cpf')", exception.Message);
                }
            }

            [Fact]
            public void Test_Get_PessoaFisica_ByCPF()
            {
                // Arrange
                var options = new DbContextOptionsBuilder<dbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase_GetByCPF")
                    .Options;

                using (var context = new dbContext(options))
                {
                    var repository = new PessoaFisicaRepositorio(context);
                    var cpfToSearch = "12345678900";

                    var mockData = new List<PessoaFisica>
                {
                    new PessoaFisica { CPF = cpfToSearch, NomeCompleto = "Teste", DataDeNascimento = DateTime.Now, Endereco = "Rua Teste" }
                };

                    context.PessoasFisicas.AddRange(mockData);
                    context.SaveChanges();

                    // Act
                    var result = repository.Get(cpfToSearch);

                    // Assert
                    Assert.NotNull(result);
                    Assert.Single(result);
                    Assert.Equal(cpfToSearch, result.First().CPF);
                }
            }

            [Fact]
            public void Test_Get_PessoaFisica_All()
            {
                // Arrange
                var options = new DbContextOptionsBuilder<dbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase_GetAll")
                    .Options;

                using (var context = new dbContext(options))
                {
                    var repository = new PessoaFisicaRepositorio(context);

                    var mockData = new List<PessoaFisica>
                {
                    new PessoaFisica { CPF = "12345678900", NomeCompleto = "Teste 1", DataDeNascimento = DateTime.Now, Endereco = "Rua Teste 1" },
                    new PessoaFisica { CPF = "09876543210", NomeCompleto = "Teste 2", DataDeNascimento = DateTime.Now, Endereco = "Rua Teste 2" }
                };

                    context.PessoasFisicas.AddRange(mockData);
                    context.SaveChanges();

                    // Act
                    var result = repository.Get(null);

                    // Assert
                    Assert.NotNull(result);
                    Assert.Equal(2, result.Count);
                }
            }

            [Fact]
            public void Test_Add_PessoaFisica_InvalidCPF()
            {
                // Arrange
                var options = new DbContextOptionsBuilder<dbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase_AddInvalidCPF")
                    .Options;

                using (var context = new dbContext(options))
                {
                    var repository = new PessoaFisicaRepositorio(context);
                    var pessoaFisica = new PessoaFisica
                    {
                        CPF = "123", // CPF inválido
                        NomeCompleto = "Teste",
                        DataDeNascimento = new DateTime(1990, 1, 1),
                        Endereco = "Rua Teste",
                        Contatos = new List<Contato>()
                {
                    new Contato { Email = "email@teste.com", Telefone = "123456789" }
                }
                    };

                    // Act & Assert
                    var exception = Assert.Throws<ArgumentException>(() => repository.Add(pessoaFisica));
                    Assert.Equal("CPF é obrigatório, deve ter 11 caracteres e ser um CPF válido.", exception.Message);
                }
            }

            [Fact]
            public void Test_Add_PessoaFisica_FutureDateOfBirth()
            {
                // Arrange
                var options = new DbContextOptionsBuilder<dbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase_AddFutureDateOfBirth")
                    .Options;

                using (var context = new dbContext(options))
                {
                    var repository = new PessoaFisicaRepositorio(context);
                    var pessoaFisica = new PessoaFisica
                    {
                        CPF = "12345678900",
                        NomeCompleto = "Teste",
                        DataDeNascimento = DateTime.Now.AddDays(1), // Data de nascimento no futuro
                        Endereco = "Rua Teste",
                        Contatos = new List<Contato>()
                {
                    new Contato { Email = "email@teste.com", Telefone = "123456789" }
                }
                    };

                    // Act & Assert
                    var exception = Assert.Throws<ArgumentException>(() => repository.Add(pessoaFisica));
                    Assert.Equal("DataDeNascimento deve ser uma data válida e anterior à data atual.", exception.Message);
                }
            }

            [Fact]
            public void Test_Add_PessoaFisica_MissingRequiredFields()
            {
                // Arrange
                var options = new DbContextOptionsBuilder<dbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase_AddMissingRequiredFields")
                    .Options;

                using (var context = new dbContext(options))
                {
                    var repository = new PessoaFisicaRepositorio(context);
                    var pessoaFisica = new PessoaFisica
                    {
                        CPF = "12345678900",
                        // NomeCompleto não fornecido
                        DataDeNascimento = new DateTime(1990, 1, 1),
                        Endereco = "Rua Teste",
                        Contatos = null // Contatos não fornecidos
                    };

                    // Act & Assert
                    var exception = Assert.Throws<ArgumentException>(() => repository.Add(pessoaFisica));
                    Assert.Equal("NomeCompleto é obrigatório e deve ter no máximo 200 caracteres.", exception.Message);
                }
            }

            [Fact]
            public void Test_Add_Contato_InvalidEmail()
            {
                // Arrange & Act & Assert
                var exception = Assert.Throws<ArgumentException>(() => Validations.ValidateContato(new Contato { Email = "invalidemail", Telefone = "123456789" }));
                Assert.Equal("Email é obrigatório e deve ser um email válido.", exception.Message);
            }

            [Fact]
            public void Test_Add_Contato_MissingPhoneNumber()
            {
                // Arrange & Act & Assert
                var exception = Assert.Throws<ArgumentException>(() => Validations.ValidateContato(new Contato { Email = "email@teste.com", Telefone = null }));
                Assert.Equal("Telefone é obrigatório.", exception.Message);
            }
        }
    }
}