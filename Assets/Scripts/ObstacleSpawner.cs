using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // Referencia al prefab de los obstaculos
    public LayerMask groundLayer; // Capa del suelo para detectar donde poner los obstaculos 
    public LayerMask obstacleLayer; // Capa de obstaculos para evitar poner mas de uno en el mismo lugar
    public LayerMask noObstacleLayer; // Capa de zonas donde no se pueden poner obstaculos
    private bool spawnActivated = false; // Flag para activar el spawn de obstaculos
    private int obstacleCount = 0; // Cantidad de obstaculos colocados
    private int maxObstacles = 5; // Maxima cantidad de obstaculos que se pueden colocar
    private GameObject[] placedObstacles; // Arreglo de obstaculos colocados para poder eliminarlos
    public InputAction mousePointerAction; // InputAction para obtener la posicion del mouse
    public InputAction clickAction; // InputAction para detectar el click del mouse

    void Start()
    {
        // Inicializamos el arreglo de obstaculos colocados con el tamaño maximo
        placedObstacles = new GameObject[maxObstacles];
    }

    private void OnEnable()
    {
        // Habilitamos las input actions cuando el script se habilita
        mousePointerAction.Enable();
        clickAction.Enable();
    }

    private void OnDisable()
    {
        // Deshabilitamos las input actions cuando el script se deshabilita
        mousePointerAction.Disable();
        clickAction.Disable();
    }

    void Update()
    {
        // Verificamos si el mouse esta sobre un boton para no colocar obstaculos
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // Obtenemos la posicion del mouse y hacemos un raycast para detectar el suelo
        Vector2 mousePosition = mousePointerAction.ReadValue<Vector2>();
        Ray r = Camera.main.ScreenPointToRay(mousePosition);
        // Si el raycast colisiona con el suelo
        if (Physics.Raycast(r, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            // Tomamos la posicion del centro del cubo de suelo y calculamos la posicion en la cual se pega el obstaculo
            Vector3 groundCenter = hit.transform.position;
            Vector3 snapPosition = new Vector3(Mathf.Round(groundCenter.x), groundCenter.y + 1f, Mathf.Round(groundCenter.z));
            // Si se hace click, el spawn esta activado y no se ha llegado al maximo de obstaculos
            if (clickAction.triggered && spawnActivated && obstacleCount < maxObstacles)
            {
                // Creamos un cubo invisible de prueba para verificar si hay espacio libre para colocar el obstaculo
                Vector3 testPosition = new Vector3(0.4f, 0.4f, 0.4f);
                // Testeamos si el cubo invisible colisiona con algo (con otro obstaculo o con alguna de las zonas donde no se pueden colocar obstaculos)
                bool noObstaclesZone = Physics.CheckBox(snapPosition, testPosition, Quaternion.identity, noObstacleLayer);
                bool occupiedSpace = Physics.CheckBox(snapPosition, testPosition, Quaternion.identity, obstacleLayer);
                // Si estas pruebas pasan, colocamos el obstaculo en la posicion calculada y aumentamos el contador de obstaculos
                if (!noObstaclesZone && !occupiedSpace)
                {
                    placedObstacles[obstacleCount] = Instantiate(obstaclePrefab, snapPosition, Quaternion.identity);
                    obstacleCount++;
                }
            }
        }
    }

    public void RemoveObstacles()
    {
        // Eliminamos todos los obstaculos colocados y reiniciamos el contador
        for (int i = 0; i < obstacleCount; i++)
        {
            Destroy(placedObstacles[i]);
        }
        obstacleCount = 0;
    }

    public void ActivateSpawning()
    {
        // Activamos el spawn de obstaculos
        spawnActivated = true;
    }
}
