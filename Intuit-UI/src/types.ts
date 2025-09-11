export type ClientReadDto = {
    clientId: number;
    firstName: string;
    lastName: string;
    birthDate: string; // YYYY-MM-DD
    cuit: string;
    address?: string | null;
    mobile: string;
    email: string;
};

export type ClientCreateDto = Omit<ClientReadDto, "clientId">;
export type ClientUpdateDto = ClientReadDto;
