using UnityEngine;

public class VoxelGridVisualizer : MonoBehaviour
{
    [Header("Настройки сетки")]
    [Tooltip("Размер одного вокселя (кубика)")]
    public float voxelSize = 1f;

    [Tooltip("Общий размер зоны сканирования")]
    public Vector3 gridVolume = new Vector3(10f, 5f, 10f);

    [Header("Настройки физики")]
    [Tooltip("Слои, которые считаются препятствиями")]
    public LayerMask obstacleMask;

    [Header("Настройки отображения")]
    [Tooltip("Показывать только непроходимые воксели?")]
    public bool showOnlyObstacles = false;

    [Range(0f, 1f)]
    public float wireframeAlpha = 0.3f;

    // Метод отрисовки Gizmos работает прямо в окне редактора Scene
    private void OnDrawGizmos()
    {
        // Вычисляем количество вокселей по каждой оси
        int countX = Mathf.FloorToInt(gridVolume.x / voxelSize);
        int countY = Mathf.FloorToInt(gridVolume.y / voxelSize);
        int countZ = Mathf.FloorToInt(gridVolume.z / voxelSize);

        // Начальная точка — левый нижний ближний угол зоны относительно позиции объекта
        Vector3 startPos = transform.position - (gridVolume / 2f) + new Vector3(voxelSize, voxelSize, voxelSize) * 0.5f;

        // Перебираем трехмерное пространство сетки
        for (int x = 0; x < countX; x++)
        {
            for (int y = 0; y < countY; y++)
            {
                for (int z = 0; z < countZ; z++)
                {
                    // Вычисляем центр текущего вокселя
                    Vector3 voxelCenter = startPos + new Vector3(x * voxelSize, y * voxelSize, z * voxelSize);

                    // Размер проверяемой зоны (чуть меньше вокселя, чтобы избежать ложных стыков)
                    Vector3 checkHalfExtents = Vector3.one * (voxelSize * 0.49f);

                    // Проверяем, пересекается ли воксель с объектами из obstacleMask
                    bool isObstacle = Physics.CheckBox(voxelCenter, checkHalfExtents, Quaternion.identity, obstacleMask);

                    // Логика визуализации
                    if (isObstacle)
                    {
                        // Красный цвет для препятствий
                        Gizmos.color = new Color(1f, 0f, 0f, 0.6f);
                        Gizmos.DrawCube(voxelCenter, Vector3.one * voxelSize);

                        Gizmos.color = Color.red;
                        Gizmos.DrawWireCube(voxelCenter, Vector3.one * voxelSize);
                    }
                    else if (!showOnlyObstacles)
                    {
                        // Зеленый цвет для свободного пространства
                        Gizmos.color = new Color(0f, 1f, 0f, wireframeAlpha);
                        Gizmos.DrawWireCube(voxelCenter, Vector3.one * voxelSize);
                    }
                }
            }
        }

        // Отрисовка общей границы зоны сканирования
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, gridVolume);
    }
}
