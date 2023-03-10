namespace GatheringEvents.Application.InvitationUseCases.AcceptInvitation;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

using GatheringEvents.Domain.Repositories;
using GatheringEvents.Application.Abstractions;
using GatheringEvents.Domain.Entities;
using GatheringEvents.Domain.Types;

public sealed class AcceptInvitationHandler
{
    // Command
    public sealed record AcceptInvitationCommand(
        Guid InvitationId): IRequest<Either<Attendee, Error>>;

    // Handler
    internal sealed class Handler : IRequestHandler<AcceptInvitationCommand, Either<Attendee, Error>>
    {
        private readonly IAttendeeRepository _attendeeRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IGatheringRepository _gatheringRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUnitOfWork _unitOfWork;
        

        public Handler(
            IAttendeeRepository attendeeRepository,
            IMemberRepository memberRepository,
            IGatheringRepository gatheringRepository,
            IInvitationRepository invitationRepository,
            IUnitOfWork unitOfWork)
        {
            _attendeeRepository = attendeeRepository;
            _memberRepository = memberRepository;
            _gatheringRepository = gatheringRepository;
            _invitationRepository = invitationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Either<Attendee, Error>> Handle(
            AcceptInvitationCommand request, 
            CancellationToken cancellationToken)
        {
            var invitation = 
                await _invitationRepository.GetByIdAsync(
                    request.InvitationId, 
                    cancellationToken);

            if (invitation is null) { 
                return Either<Attendee, Error>.Fail(
                    Error.BuildNewArgumentNullException(
                        operation: nameof(AcceptInvitationCommand), 
                        parameterName: nameof(invitation)));
            }

            if (invitation.Status != InvitationStatus.Pending) {
                return Either<Attendee, Error>.Fail(
                    Error.BuildNewInvalidOperationException(
                        operation: nameof(AcceptInvitationCommand), 
                        status: invitation.Status));
            }

            var member = 
                await _memberRepository.GetByIdAsync(
                    invitation.MemberId, 
                    cancellationToken);

            var gathering = 
                await _gatheringRepository.GetByIdWithOwnerAsync(
                    invitation.GatheringId,
                    cancellationToken);

            if (member is null) { 
                return Either<Attendee, Error>.Fail(
                    Error.BuildNewArgumentNullException(
                        operation: nameof(AcceptInvitationCommand), 
                        parameterName: nameof(member))); 
            }
            
            if (gathering is null) {
                return Either<Attendee, Error>.Fail(
                    Error.BuildNewArgumentNullException(
                        operation: nameof(AcceptInvitationCommand), 
                        parameterName: nameof(gathering)));
            }
            
            var attendee = gathering.AcceptInvitation(invitation);
            
            if (attendee is null) {
                return Either<Attendee, Error>.Fail(
                    Error.BuildNewArgumentNullException(
                        operation: nameof(AcceptInvitationCommand), 
                        parameterName: nameof(attendee)));
            }

            _attendeeRepository.Add(attendee); 
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Either<Attendee, Error>.Ok(attendee);
        }
    }
}