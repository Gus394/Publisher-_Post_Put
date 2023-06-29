using Univali.Api.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Univali.Api.Features.Publishers.Commands.CreatePublisher;
using Univali.Api.Features.Publishers.Commands.UpdatePublisher;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/publishers")]
public class PublishersController : MainController
{
    private readonly IMapper _mapper;
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMediator _mediator;

    public PublishersController (IMapper mapper, IPublisherRepository publisherRepository, IMediator mediator) {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _publisherRepository = publisherRepository ?? throw new ArgumentNullException(nameof(publisherRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    public async Task<ActionResult<CreatePublisherDto>> CreatePublisherAsync(CreatePublisherCommand createPublisherCommand) {

        CreatePublisherDto publisherToReturn = await _mediator.Send(createPublisherCommand);
        
        return CreatedAtRoute
        (
            "GetPublisherById",
            new { id = publisherToReturn.Id },
            publisherToReturn
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePublisherAsync(UpdatePublisherCommand updatePublisherCommand, int id)
    {
        if (id != updatePublisherCommand.Id) return BadRequest();

        bool result = await _mediator.Send(updatePublisherCommand);
        if(!result) return NotFound();

        return NoContent();
    }
}