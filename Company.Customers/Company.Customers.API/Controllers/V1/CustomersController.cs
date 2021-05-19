using Microsoft.AspNetCore.Mvc;
using Company.Customers.Application.AppService.Interfaces;
using Company.Customers.Application.Request.Customer;
using Company.Customers.Application.Response.Customer;
using Company.Customers.Infra.CrossCutting.Utils;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Company.Customers.API.Controllers.V1
{
    [Route("company/v1/customers")]
    [Produces("application/json")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerAppService _customerAppService;

        public CustomersController(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OperationSuccess<CustomerResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(OperationFail<CustomerResponse>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Register([FromBody]CustomerRequest request)
        {
            var customer =  await _customerAppService.Register(request);
            if (customer is OperationFail<CustomerResponse>)
                return BadRequest(customer);

            return Ok(customer);
        }


        [HttpGet("cpf/{cpf}")]
        [ProducesResponseType(typeof(OperationSuccess<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ListByCpf(string cpf)
        {
            var customer = await _customerAppService.ListByCpf(cpf);
            if (customer is OperationFail<CustomerResponse>)
                return NotFound();

            return Ok(customer);
        }

        [HttpGet]
        [ProducesResponseType(typeof(OperationSuccess<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAll(int page)
        {
            var customer = await _customerAppService.GetAll(page);
            if (customer is OperationFail<List<CustomerResponse>>)
                return BadRequest(customer);

            return Ok(customer);
        }
    }
}
