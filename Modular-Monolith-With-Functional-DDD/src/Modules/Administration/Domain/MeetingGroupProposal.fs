namespace MyCompany.Meeting.Module.Administration.Domain

open System
open FsToolkit.ErrorHandling
open MyCompany.Meeting.Common.Domain

// Simple Types
type private MeetingGroupProposalId = Id

type private UserId = Id

// TODO: Country code, city
type private Location = ToDo

type ProposalDecision = 

// Public Types
// Entity, Aggregate Root
type InVerificationMeetingGroupProposal =
    { Id: MeetingGroupProposalId
      Name: String50
      Description: String200
      Location: Location
      ProposalUserId: UserId
      ProposalDate: DateTime }

type AcceptedMeetingGroupProposal =
    { Id: MeetingGroupProposalId
      Name: String50
      Description: String200
      Location: Location
      ProposalUserId: UserId
      ProposalDate: DateTime }

type RejectedMeetingGroupProposal =
    { Id: MeetingGroupProposalId
      Name: String50
      Description: String200
      Location: Location
      ProposalUserId: UserId
      ProposalDate: DateTime }

// Public Command Types
type RequestVerificationMeetingGroupProposal = ToDo

type AcceptMeetingGroupProposal = InVerificationMeetingGroupProposal -> AcceptedMeetingGroupProposal

type RejectMeetingGroupProposal = InVerificationMeetingGroupProposal -> RejectedMeetingGroupProposal