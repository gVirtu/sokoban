using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PreviewPanel : MonoBehaviour
{
    public static PreviewPanel Instance;
    public float orbitSpeed = 10f;
    public GameObject PreviewBuilder;
    public CinemachineVirtualCameraBase orbitCam;

    private CinemachineOrbitalTransposer orbitalTransposer;

    private GameObject CurrentPreview;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        orbitalTransposer = (orbitCam as CinemachineVirtualCamera).GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void Update()
    {
        if (orbitalTransposer != null)
            orbitalTransposer.m_XAxis.Value += Time.deltaTime * orbitSpeed;
    }
    public void RefreshPreview()
    {
        if (CurrentPreview != null)
            Destroy(CurrentPreview);

        CurrentPreview = Instantiate(PreviewBuilder);
        CurrentPreview.GetComponent<LevelBuilder>().cinemachineCam = orbitCam;
    }
}
