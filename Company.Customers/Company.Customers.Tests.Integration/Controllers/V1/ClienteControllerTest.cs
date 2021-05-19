using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Moq;
using Company.Customers.API;
using Company.Customers.Application.AppService;
using Company.Customers.Application.AppService.Interfaces;
using Company.Customers.Application.Request.Customer;
using Company.Customers.Application.Response.Customer;
using Company.Customers.Infra.CrossCutting.Utils;
using Company.Customers.Infra.CrossCutting.Utils.Interfaces;
using Company.Customers.Infra.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Company.Customers.Tests.Integration.Controllers.V1
{
    public class CustomerControllerTest
    {

        [Fact]
        public async Task Se_CadastrarCustomer_Entao_RetornarIdDeCorrelacaoEmFormatoGuid()
        {
            const string cpf = "12345789";
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICustomerAppService>();
                    mockAppService.Setup(x => x.Register(null))
                                    .Returns(Task.FromResult(default(IOperation<CustomerResponse>)));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync($"/company/v1/customer/cpf/{cpf}");
            response.Headers.TryGetValues("x-correlation-id", out var valuesHeadrs);
            string correlationId = valuesHeadrs.First();
            var ehGuid = Guid.TryParse(correlationId, out var correlationIdGuid);
            Assert.NotNull(correlationId);
            Assert.True(ehGuid);

        }

        [Fact]
        public async Task Se_CustomerNaoCadastrado_Entao_Retornar400()
        {
            CustomerRequest request = null;
            var requestBody = new HttpRequestMessage(HttpMethod.Post,"")
            {
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };


            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICustomerAppService>();
                    mockAppService.Setup(x => x.Register(request))
                                    .Returns(Task.FromResult(Result.CreateFailure<CustomerResponse>("")));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.PostAsync($"/company/v1/customer", requestBody.Content);
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Se_CustomerCadastrado_Entao_Retornar200()
        {
            var requestBody = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = new StringContent(JsonSerializer.Serialize(default(CustomerRequest)),
                                            Encoding.UTF8, "application/json")
            };


            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICustomerAppService>();
                    mockAppService.Setup(x => x.Register(null))
                                  .Returns(Task.FromResult(Result.CreateSuccess(new CustomerResponse())));

                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.PostAsync($"/company/v1/customer", requestBody.Content);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task Se_CpfNaoExistir_Entao_Retornar404()
        {
            const string cpf = "12345789";
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICustomerAppService>();
                    mockAppService.Setup(x => x.ListByCpf(cpf))
                                    .Returns(Task.FromResult(Result.CreateFailure<CustomerResponse>("")));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();


            var response = await _httpClient.GetAsync($"/company/v1/customer/cpf/{cpf}");
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Se_ConsultarPorCpf_Entao_RetornarIdDeCorrelacaoEmFormatoGuid()
        {
            const string cpf = "12345789";
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICustomerAppService>();
                    mockAppService.Setup(x => x.ListByCpf(cpf))
                                    .Returns(Task.FromResult(Result.CreateFailure<CustomerResponse>("")));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync($"/company/v1/customer/cpf/{cpf}");
            response.Headers.TryGetValues("x-correlation-id", out var valuesHeadrs);
            string correlationId = valuesHeadrs.First();
            var ehGuid = Guid.TryParse(correlationId, out var correlationIdGuid);
            Assert.NotNull(correlationId);
            Assert.True(ehGuid);

        }


        [Fact]
        public async Task Se_CpfExistir_Entao_Retornar200()
        {
            const string cpf = "12345789";
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICustomerAppService>();
                    mockAppService.Setup(x => x.ListByCpf(cpf))
                                    .Returns(Task.FromResult(Result.CreateSuccess(new CustomerResponse())));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync($"/company/v1/customer/cpf/{cpf}");
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }



        [Fact]
        public async Task Se_ConsultaPaginadaRetornarErro_Entao_Retornar400()
        {
            const int pagina = 1;
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICustomerAppService>();
                    mockAppService.Setup(x => x.ist(pagina))
                                    .Returns(Task.FromResult(Result.CreateFailure<List<CustomerResponse>>("")));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync($"/company/v1/customer?pagina={pagina}");
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Se_ConsultaPaginadaRetornarSucesso_Entao_Retornar200()
        {
            const int pagina = 1;
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICustomerAppService>();
                    mockAppService.Setup(x => x.ist(pagina))
                                    .Returns(Task.FromResult(Result.CreateSuccess<List<CustomerResponse>>(null)));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync($"/company/v1/customer?pagina={pagina}");
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }


        [Fact]
        public async Task Se_ConsultaPaginada_Entao_RetornarIdDeCorrelacaoEmFormatoGuid()
        {
            const int pagina = 1;
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICustomerAppService>();
                    mockAppService.Setup(x => x.ist(pagina))
                                    .Returns(Task.FromResult(Result.CreateFailure<List<CustomerResponse>>(null)));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync($"/company/v1/customer?pagina={pagina}");
            response.Headers.TryGetValues("x-correlation-id", out var valuesHeadrs);
            string correlationId = valuesHeadrs.First();
            var ehGuid = Guid.TryParse(correlationId, out var correlationIdGuid);
            Assert.NotNull(correlationId);
            Assert.True(ehGuid);

        }
    }

}
