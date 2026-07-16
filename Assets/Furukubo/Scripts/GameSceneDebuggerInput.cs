using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSceneDebuggerInput : MonoBehaviour
{
    [Header("Input Action Names")]
    [SerializeField] private string _debugMapName;
    [SerializeField] private string _debug1ActName;
    [SerializeField] private string _debug2ActName;
    [SerializeField] private string _debug3ActName;
    [SerializeField] private string _debug4ActName;
    [SerializeField] private string _debug5ActName;
    [SerializeField] private string _debug6ActName;
    [SerializeField] private string _debug7ActName;
    [SerializeField] private string _debug8ActName;
    [SerializeField] private string _debug9ActName;
    [SerializeField] private string _debug0ActName;
    [Header("References")]
    [SerializeField] private GameSceneDebugger _debugger;
    [SerializeField] private InputActionAsset _inputActionAsset;

    private InputActionMap _debugMap;
    private InputAction _debug1Act;
    private InputAction _debug2Act;
    private InputAction _debug3Act;
    private InputAction _debug4Act;
    private InputAction _debug5Act;
    private InputAction _debug6Act;
    private InputAction _debug7Act;
    private InputAction _debug8Act;
    private InputAction _debug9Act;
    private InputAction _debug0Act;

    private void Awake()
    {
        _debugMap = _inputActionAsset?.FindActionMap(_debugMapName);
        _debug1Act = _debugMap?.FindAction(_debug1ActName);
        _debug2Act = _debugMap?.FindAction(_debug2ActName);
        _debug3Act = _debugMap?.FindAction(_debug3ActName);
        _debug4Act = _debugMap?.FindAction(_debug4ActName);
        _debug5Act = _debugMap?.FindAction(_debug5ActName);
        _debug6Act = _debugMap?.FindAction(_debug6ActName);
        _debug7Act = _debugMap?.FindAction(_debug7ActName);
        _debug8Act = _debugMap?.FindAction(_debug8ActName);
        _debug9Act = _debugMap?.FindAction(_debug9ActName);
        _debug0Act = _debugMap?.FindAction(_debug0ActName);
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;

        _debugMap?.Enable();

        if (_debug1Act != null) _debug1Act.performed += Debug1;
        if (_debug2Act != null) _debug2Act.performed += Debug2;
        if (_debug3Act != null) _debug3Act.performed += Debug3;
        if (_debug4Act != null) _debug4Act.performed += Debug4;
        if (_debug5Act != null) _debug5Act.performed += Debug5;
        if (_debug6Act != null) _debug6Act.performed += Debug6;
        if (_debug7Act != null) _debug7Act.performed += Debug7;
        if (_debug8Act != null) _debug8Act.performed += Debug8;
        if (_debug9Act != null) _debug9Act.performed += Debug9;
        if (_debug0Act != null) _debug0Act.performed += Debug0;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;

        _debugMap?.Disable();

        if (_debug1Act != null) _debug1Act.performed -= Debug1;
        if (_debug2Act != null) _debug2Act.performed -= Debug2;
        if (_debug3Act != null) _debug3Act.performed -= Debug3;
        if (_debug4Act != null) _debug4Act.performed -= Debug4;
        if (_debug5Act != null) _debug5Act.performed -= Debug5;
        if (_debug6Act != null) _debug6Act.performed -= Debug6;
        if (_debug7Act != null) _debug7Act.performed -= Debug7;
        if (_debug8Act != null) _debug8Act.performed -= Debug8;
        if (_debug9Act != null) _debug9Act.performed -= Debug9;
        if (_debug0Act != null) _debug0Act.performed -= Debug0;
    }

    private void OnSceneChanged(Scene from, Scene to)
    {
        _debugMap?.Enable();
    }

    private void Debug1(InputAction.CallbackContext ctx)
    {
        if (_debugger == null) return;
        _debugger.Debug1();
        Debug.Log("Debug 1");
    }

    private void Debug2(InputAction.CallbackContext ctx)
    {
        if (_debugger == null) return;
        _debugger.Debug2();
        Debug.Log("Debug 2");
    }

    private void Debug3(InputAction.CallbackContext ctx)
    {
        if (_debugger == null) return;
        _debugger.Debug3();
        Debug.Log("Debug 3");
    }

    private void Debug4(InputAction.CallbackContext ctx)
    {
        if (_debugger == null) return;
        _debugger.Debug4();
        Debug.Log("Debug 4");
    }

    private void Debug5(InputAction.CallbackContext ctx)
    {
        if (_debugger == null) return;
        _debugger.Debug5();
        Debug.Log("Debug 5");
    }

    private void Debug6(InputAction.CallbackContext ctx)
    {
        if (_debugger == null) return;
        _debugger.Debug6();
        Debug.Log("Debug 6");
    }

    private void Debug7(InputAction.CallbackContext ctx)
    {
        if (_debugger == null) return;
        _debugger.Debug7();
        Debug.Log("Debug 7");
    }

    private void Debug8(InputAction.CallbackContext ctx)
    {
        if (_debugger == null) return;
        _debugger.Debug8();
        Debug.Log("Debug 8");
    }

    private void Debug9(InputAction.CallbackContext ctx)
    {
        if (_debugger == null) return;
        _debugger.Debug9();
        Debug.Log("Debug 9");
    }

    private void Debug0(InputAction.CallbackContext ctx)
    {
        if (_debugger == null) return;
        _debugger.Debug0();
        Debug.Log("Debug 0");
    }
}
