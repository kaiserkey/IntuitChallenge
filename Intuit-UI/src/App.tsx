import { useEffect, useMemo, useState } from "react";
import {
    AppBar,
    Box,
    Button,
    Container,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    IconButton,
    Snackbar,
    Stack,
    TextField,
    Toolbar,
    Typography,
    Alert,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import AddIcon from "@mui/icons-material/Add";
import SearchIcon from "@mui/icons-material/Search";
import { createClient, deleteClient, getClients } from "./api";
import type { ClientReadDto } from "./types";

export default function App() {
    const [rows, setRows] = useState<ClientReadDto[]>([]);
    const [q, setQ] = useState("");

    const [loading, setLoading] = useState(false);
    const [open, setOpen] = useState(false);
    const [err, setErr] = useState<string | null>(null);
    const [ok, setOk] = useState<string | null>(null);

    const [form, setForm] = useState({
        firstName: "",
        lastName: "",
        birthDate: "",
        cuit: "",
        address: "",
        mobile: "",
        email: "",
    });

    const disabledCreate = useMemo(() => {
        const { firstName, lastName, birthDate, cuit, mobile, email } = form;
        return (
            !firstName || !lastName || !birthDate || !cuit || !mobile || !email
        );
    }, [form]);

    async function load() {
        try {
            setLoading(true);
            const data = await getClients(q);
            setRows(data);
        } catch (e: any) {
            setErr(e.message ?? "Error cargando clientes");
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        const t = setTimeout(() => load(), 300);
        return () => clearTimeout(t);
    }, [q]);

    async function onCreate() {
        try {
            await createClient(form);
            setOpen(false);
            setForm({
                firstName: "",
                lastName: "",
                birthDate: "",
                cuit: "",
                address: "",
                mobile: "",
                email: "",
            });
            setOk("Cliente creado");
            await load();
        } catch (e: any) {
            setErr(e.message ?? "Error creando cliente");
        }
    }

    async function onDelete(id: number) {
        if (!confirm("¿Eliminar cliente?")) return;
        try {
            await deleteClient(id);
            setOk("Cliente eliminado");
            await load();
        } catch (e: any) {
            setErr(e.message ?? "Error eliminando cliente");
        }
    }

    return (
        <Box>
            <AppBar position="static">
                <Toolbar>
                    <Typography variant="h6" sx={{ flexGrow: 1 }}>
                        Intuit Challenge – Clientes
                    </Typography>

                    <Stack direction="row" spacing={1} alignItems="center">
                        <SearchIcon />
                        <TextField
                            size="small"
                            placeholder="Buscar (nombre o apellido)"
                            value={q}
                            onChange={(e) => setQ(e.target.value)}
                            sx={{
                                bgcolor: "white",
                                borderRadius: 1,
                                minWidth: 280,
                            }}
                        />

                        <Button
                            color="inherit"
                            startIcon={<AddIcon />}
                            onClick={() => setOpen(true)}
                        >
                            Nuevo Cliente
                        </Button>
                    </Stack>
                </Toolbar>
            </AppBar>

            <Container sx={{ py: 3 }}>
                {loading ? (
                    <Typography>Cargando…</Typography>
                ) : (
                    <TableContainer component={Paper}>
                        <Table size="small">
                            <TableHead>
                                <TableRow>
                                    <TableCell>Nombre</TableCell>
                                    <TableCell>Apellido</TableCell>
                                    <TableCell>CUIT</TableCell>
                                    <TableCell>Email</TableCell>
                                    <TableCell>Teléfono</TableCell>
                                    <TableCell width={80} align="right">
                                        Acciones
                                    </TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {rows.map((c) => (
                                    <TableRow key={c.clientId} hover>
                                        <TableCell>{c.firstName}</TableCell>
                                        <TableCell>{c.lastName}</TableCell>
                                        <TableCell>{c.cuit}</TableCell>
                                        <TableCell>{c.email}</TableCell>
                                        <TableCell>{c.mobile}</TableCell>
                                        <TableCell align="right">
                                            <IconButton
                                                color="error"
                                                size="small"
                                                onClick={() =>
                                                    onDelete(c.clientId)
                                                }
                                            >
                                                <DeleteIcon />
                                            </IconButton>
                                        </TableCell>
                                    </TableRow>
                                ))}

                                {rows.length === 0 && (
                                    <TableRow>
                                        <TableCell colSpan={6} align="center">
                                            Sin resultados
                                        </TableCell>
                                    </TableRow>
                                )}
                            </TableBody>
                        </Table>
                    </TableContainer>
                )}
            </Container>

            <Dialog
                open={open}
                onClose={() => setOpen(false)}
                fullWidth
                maxWidth="sm"
            >
                <DialogTitle>Nuevo cliente</DialogTitle>
                <DialogContent>
                    <Stack mt={1} spacing={2}>
                        <Stack
                            direction={{ xs: "column", sm: "row" }}
                            spacing={2}
                        >
                            <TextField
                                label="Nombre"
                                value={form.firstName}
                                onChange={(e) =>
                                    setForm((f) => ({
                                        ...f,
                                        firstName: e.target.value,
                                    }))
                                }
                                required
                                fullWidth
                            />
                            <TextField
                                label="Apellido"
                                value={form.lastName}
                                onChange={(e) =>
                                    setForm((f) => ({
                                        ...f,
                                        lastName: e.target.value,
                                    }))
                                }
                                required
                                fullWidth
                            />
                        </Stack>
                        <TextField
                            label="Fecha de nacimiento"
                            type="date"
                            InputLabelProps={{ shrink: true }}
                            value={form.birthDate}
                            onChange={(e) =>
                                setForm((f) => ({
                                    ...f,
                                    birthDate: e.target.value,
                                }))
                            }
                            required
                        />
                        <TextField
                            label="CUIT"
                            value={form.cuit}
                            onChange={(e) =>
                                setForm((f) => ({ ...f, cuit: e.target.value }))
                            }
                            required
                        />
                        <TextField
                            label="Dirección"
                            value={form.address}
                            onChange={(e) =>
                                setForm((f) => ({
                                    ...f,
                                    address: e.target.value,
                                }))
                            }
                        />
                        <TextField
                            label="Móvil"
                            value={form.mobile}
                            onChange={(e) =>
                                setForm((f) => ({
                                    ...f,
                                    mobile: e.target.value,
                                }))
                            }
                            required
                        />
                        <TextField
                            label="Email"
                            type="email"
                            value={form.email}
                            onChange={(e) =>
                                setForm((f) => ({
                                    ...f,
                                    email: e.target.value,
                                }))
                            }
                            required
                        />
                    </Stack>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setOpen(false)}>Cancelar</Button>
                    <Button
                        variant="contained"
                        onClick={onCreate}
                        disabled={disabledCreate}
                    >
                        Crear
                    </Button>
                </DialogActions>
            </Dialog>

            <Snackbar
                open={!!err}
                autoHideDuration={4000}
                onClose={() => setErr(null)}
            >
                <Alert severity="error" onClose={() => setErr(null)}>
                    {err}
                </Alert>
            </Snackbar>
            <Snackbar
                open={!!ok}
                autoHideDuration={2500}
                onClose={() => setOk(null)}
            >
                <Alert severity="success" onClose={() => setOk(null)}>
                    {ok}
                </Alert>
            </Snackbar>
        </Box>
    );
}
