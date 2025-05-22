using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class UpgradesController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private CinemachineCamera _upgradeCamera;
    [SerializeField] private GameObject _upgradeMenuUI;
    [SerializeField] private GameObject _powersSelectionScreen;
    [SerializeField] private List<Transform> _slotsToVisualize;
    [SerializeField] private Animator _playerAnimator;

    private int _selectedSlotNumber;
    private PowerVisual[] _equippedPowers = new PowerVisual[6];

    public void InitializeUpgradeMenu()
    {
        _upgradeCamera.Priority = 2;
        _upgradeMenuUI.SetActive(true);
        StartCoroutine(MenuShowRoutine());
    }

    public void CloseUpgradeMenu()
    {
        _playerController.SetMovement(true);
        _upgradeCamera.Priority = 0;
        _upgradeMenuUI.SetActive(false);
        _playerAnimator.SetTrigger("cocoonExit");
    }

    public void OpenPowerSelection(int slotNumber)
    {
        _selectedSlotNumber = slotNumber;
        _powersSelectionScreen.SetActive(true);
    }

    public void ClosePowerSelection()
    {
        _powersSelectionScreen.SetActive(false);
    }

    public void ApplyUpgradeInCurrentSlot(PowerVisual powerVisual, GameObject upgradeVisual)
    {
        Instantiate(upgradeVisual, _slotsToVisualize[_selectedSlotNumber]);
        _equippedPowers[_selectedSlotNumber] = powerVisual;
        ClosePowerSelection();
    }

    public void ClearUpgradeInCurrentSlot()
    {
        if (_slotsToVisualize[_selectedSlotNumber].childCount != 0)
        {
            GameObject toDestroy = _slotsToVisualize[_selectedSlotNumber].GetChild(0).gameObject;
            Destroy(toDestroy);
        }
        if (_equippedPowers[_selectedSlotNumber] != null)
        {
            _equippedPowers[_selectedSlotNumber].MarkUnequipped();
        }
        _equippedPowers[_selectedSlotNumber] = null;
    }

    private IEnumerator MenuShowRoutine()
    {
        yield return new WaitForSeconds(2f);
        _playerAnimator.SetTrigger("cocoonEnter");
    }
}
