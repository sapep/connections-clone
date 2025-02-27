export interface Connection {
  id: number
  connectionType: string
  words: Word[]
}

export interface Word {
  id: number
  connectionId: number
  value: string
}
