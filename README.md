# CronProject

12/06/2024 - Primer commit donde se realizó conexión con base de datos local y creación de archivo correspondiente al modelo para su posterior migración.


******************************************************


13/06/2024:

Añadi ID indicando que es autoincremental en archivo del Modelo (DataRecord) y archivo del DbSet(entidad).

Cree carpeta Controllers donde estara la logica de comunicacion entre las peticiones que hara el cliente (navegador)

Cree carpeta para los servicios en el cual añadi archivo CsvImportService.cs el cual leera los datos del excel y los registrara en la BD

Tuve que añadir clase DataRecordWithoutId en archivo de importacion del CSV a la base de datos ya que solicitaba el campo ID siendo que es auto incremental

Tambien añadi configuracion para el Csv en el mismo archivo.

Tuve que configurar la fecha usada en CsvHelper ya que por defecto usa un formato con DateTime y en el csv solo entregare fecha.

Se logra guardar datos de CSV en BD


*****************************************
14/06/2024 - 15/06/2024:

Añadi nuevo campo en tabla (modelo) DataRecord para identificar los datos subidos en el proceso correspondiente y asi obtener la data para el csv de exportacion.

Cree 2 metodos uno para importar y otro para exportar los csv, para asi llamarlo desde un metodo padre para tener mas orden. Tambien de esta forma le podia entregar el identificador de sesion unico a las clases de importacion y exportacion.

Para este campo use un GUID que suele ser usado para identificadores unicos en C#

Para el procesamiento paralelo. Al inicio solo implemente el Task.run usando la misma instancia de DbContext en las tareas paralelas, lo que me ocasionaba problemas (no insertaba todos los registros).

Entonces:

 Entonces opte por "Crear un nuevo alcance DbContext (scope) para cada lote" solucionando el problema.

cree variable instanciada en Program.cs donde se define la cantidad de registros que tendra cada lote

Para la asignacion de lotes aplique logica de dividir el indice de cada registro por el tamaño del lote (Si batchSize es 100, los indices de 0 a 99 estarab en el primer grupo (clave 0), los índices de 100 a 199 estarán en el segundo grupo (clave 1))


*****************************************








