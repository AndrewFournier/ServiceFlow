import type { ServiceOffering } from './types'

const API_URL =
  import.meta.env.VITE_API_URL ?? 'http://localhost:5003'

export async function getServices(): Promise<ServiceOffering[]> {
  const response = await fetch(`${API_URL}/api/services`)

  if (!response.ok) {
    throw new Error('Could not load services.')
  }

  return response.json()
}