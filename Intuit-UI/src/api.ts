const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? "https://localhost:7178";

async function http<T>(input: RequestInfo, init?: RequestInit): Promise<T> {
    const res = await fetch(input, {
        ...init,
        headers: {
            "Content-Type": "application/json",
            ...(init?.headers || {}),
        },
    });
    if (!res.ok) {
        let details: any = undefined;
        try {
            details = await res.json();
        } catch {
            details = await res.text();
        }
        throw new Error(
            typeof details === "string"
                ? details
                : details?.message ?? `HTTP ${res.status}`,
        );
    }
    const text = await res.text();
    return (text ? JSON.parse(text) : undefined) as T;
}

export async function getClients(name?: string) {
    const url = name?.trim()
        ? `${BASE_URL}/api/search?name=${encodeURIComponent(name.trim())}`
        : `${BASE_URL}/api`;
    return http<any[]>(url);
}

export async function createClient(dto: any) {
    return http<any>(`${BASE_URL}/api`, {
        method: "POST",
        body: JSON.stringify(dto),
    });
}

export async function deleteClient(id: number) {
    return http<void>(`${BASE_URL}/api/${id}`, { method: "DELETE" });
}

export async function updateClient(id: number, dto: any) {
    return http<any>(`${BASE_URL}/api/${id}`, {
        method: "PUT",
        body: JSON.stringify(dto),
    });
}
