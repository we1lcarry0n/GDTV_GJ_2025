using UnityEngine;
using UnityEngine.UI;

public class PowerVisual : MonoBehaviour
{
    [SerializeField] private UpgradesController _upgradeController;
    [SerializeField] private GameObject _powerVisualObject;
    [SerializeField] private PowerLogic _powerLogic;

    [SerializeField] private Button _buttonControls;
    [SerializeField] private GameObject _equippedMask;

    public void ApplyPower()
    {
        _upgradeController.ClearUpgradeInCurrentSlot();
        _upgradeController.ApplyUpgradeInCurrentSlot(this, _powerVisualObject);
        MarkEquipped();
    }

    private void MarkEquipped()
    {
        _buttonControls.interactable = false;
        _equippedMask.SetActive(true);
    }

    public void MarkUnequipped()
    {
        _buttonControls.interactable = true;
        _equippedMask.SetActive(false);
    }
}
