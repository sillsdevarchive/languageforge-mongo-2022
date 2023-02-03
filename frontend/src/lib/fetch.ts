import { start, stop } from '$lib/progress'
import type { HttpMethod } from '@sveltejs/kit/types/private'

type FetchArgs = {
	url: string,
	body?: object,
}
interface AdaptedFetchArgs extends FetchArgs {
	method: HttpMethod,
}

export async function CREATE({url, body}: FetchArgs) { return await adapted_fetch({method: 'POST'  , url, body}) }
export async function GET   ({url      }: FetchArgs) { return await adapted_fetch({method: 'GET'   , url      }) }
export async function UPDATE({url, body}: FetchArgs) { return await adapted_fetch({method: 'PUT'   , url, body}) }
export async function DELETE({url      }: FetchArgs) { return await adapted_fetch({method: 'DELETE', url      }) }

// https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API/Using_Fetch#Supplying_request_options
async function adapted_fetch({method, url, body}: AdaptedFetchArgs) {
	start(url)

	const response: Response = await fetch(url, {
		method,
		headers: {
			'content-type': 'application/json',
		},
		body: JSON.stringify(body),
	})
	.finally(() => stop(url))

	// reminder: fetch does not throw exceptions for non-200 responses (https://developer.mozilla.org/en-US/docs/Web/API/WindowOrWorkerGlobalScope/fetch)
	if (! response.ok) {
		const results = await response.json().catch(() => {}) || {}

		throw Error(results.message || response.statusText)
	}

	return await response.json()
}
