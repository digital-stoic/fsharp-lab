module Types

type Lead =
    { id: string // TODO: id
      title: string
      pipeline: string // TODO: union type
      step: string // TODO: union type
      step_id: int
      status: string // TODO: union type
      amount: float option
      probability: float option
      currency: string option // TODO: union type
      starred: bool
      remind_date: string // TODO: date
      remind_time: string option // TODO: date
      created_at: string // TODO: date
      estimated_closing_date: string option // TODO: date
      updated_at: string // TODO: date
      description: string
      html_description: string
      tags: string list
      created_from: string option // TODO: date
      closed_at: string option // TODO: date
      attachment_count: int
      created_by_id: int // TODO: id
      user_id: int // TODO: id
      client_folder_id: int option // TODO: id
      client_folder_name: string option
      team_id: int option // TODO: id
      team_name: string option }
