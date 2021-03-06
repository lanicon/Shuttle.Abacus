﻿using System;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Abacus.DataAccess;
using Shuttle.Abacus.Messages.v1;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;

namespace Shuttle.Abacus.WebApi.Controllers
{
    [RequiresPermission(SystemPermissions.Manage.Arguments)]
    [Route("api/[controller]")]
    [RequiresSession]
    public class ArgumentsController : Controller
    {
        private readonly IServiceBus _bus;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IDataRowMapper _dataRowMapper;
        private readonly IArgumentQuery _argumentQuery;

        public ArgumentsController(IServiceBus bus, IDatabaseContextFactory databaseContextFactory,
            IDataRowMapper dataRowMapper, IArgumentQuery argumentQuery)
        {
            Guard.AgainstNull(bus, nameof(bus));
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(dataRowMapper, nameof(dataRowMapper));
            Guard.AgainstNull(argumentQuery, nameof(argumentQuery));

            _bus = bus;
            _databaseContextFactory = databaseContextFactory;
            _dataRowMapper = dataRowMapper;
            _argumentQuery = argumentQuery;
        }

        [HttpPost("search")]
        public IActionResult Search([FromBody] ArgumentSearchModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _dataRowMapper.MapObjects<ArgumentModel>(
                        _argumentQuery.Search(model.Specification()))
                });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ArgumentModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            _bus.Send(new RegisterArgumentCommand
            {
                Id = model.Id,
                Name = model.Name,
                DataTypeName = model.DataTypeName
            });

            return Ok();
        }

        [HttpPost("{id}/values")]
        public IActionResult Post(Guid id, [FromBody] ArgumentValueModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            _bus.Send(new RegisterArgumentValueCommand
            { 
                ArgumentId = id,
                Value = model.Value
            });

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _dataRowMapper.MapObject<ArgumentModel>(_argumentQuery.Get(id))
                });
            }
        }

        [HttpGet("{id}/values")]
        public IActionResult Values(Guid id)
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _dataRowMapper.MapObjects<ArgumentValueModel>(_argumentQuery.Values(id))
                });
            }
        }

        [HttpDelete("{id}/values")]
        public IActionResult DeleteValue(Guid id, [FromBody] ArgumentValueModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            _bus.Send(new RemoveArgumentValueCommand
            { 
                ArgumentId = id,
                Value = model.Value
            });

            return Ok();
        }
    }
}