# CronProject

12/06/2024 - Primer commit donde se realizó conexión con base de datos local y creación de archivo correspondiente al modelo para su posterior migración.



13/06/2024:

Añadi ID indicando que es autoincremental en archivo del Modelo (DataRecord) y archivo del DbSet(entidad).

Cree carpeta Controllers donde estara la logica de comunicacion entre las peticiones que hara el cliente (navegador)

Cree carpeta para los servicios en el cual añadi archivo CsvImportService.cs el cual leera los datos del excel y los registrara en la BD

Tuve que añadir clase DataRecordWithoutId en archivo de importacion del CSV a la base de datos ya que solicitaba el campo ID siendo que es auto incremental

Tambien añadi configuracion para el Csv en el mismo archivo.

Tuve que configurar la fecha usada en CsvHelper ya que por defecto usa un formato con DateTime y en el csv solo entregare fecha.

Se logra guardar datos de CSV en BD

