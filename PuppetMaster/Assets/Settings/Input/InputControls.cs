// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Input/InputControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputControls"",
    ""maps"": [
        {
            ""name"": ""InWorld"",
            ""id"": ""7c93372b-b7f8-4fe6-a294-79686da6c6b5"",
            ""actions"": [
                {
                    ""name"": ""Fire1"",
                    ""type"": ""Button"",
                    ""id"": ""6b3fa1cb-0e1b-4704-b4b6-f7e952948c88"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire2"",
                    ""type"": ""Button"",
                    ""id"": ""ef9c8a4e-c821-4286-889b-3c22a126cd2d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""455aa3be-1803-47df-ba10-df6fd6f2ec7a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9968c8ef-d1c4-42e2-986e-b90a9b16b4b8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""12576929-f4bc-4d1f-9ee4-f189c2e2ab8f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Fire1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c29d3dbf-b1d5-4e91-a830-5aeb71cc18c8"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Fire2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""7931ed62-7ede-41a4-8a56-9242e0c24387"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a2f67d91-5069-4142-80a2-fbd777ba2709"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e5fd085d-9b18-4442-95e6-5fb07a5ded8a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""819aa0c3-e8b7-401a-8150-29a863c3ad9e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""020ce877-d28c-4a56-9f17-1caea3bf866d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ec61e89e-2bda-4230-8d4a-e9c39848e787"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // InWorld
        m_InWorld = asset.FindActionMap("InWorld", throwIfNotFound: true);
        m_InWorld_Fire1 = m_InWorld.FindAction("Fire1", throwIfNotFound: true);
        m_InWorld_Fire2 = m_InWorld.FindAction("Fire2", throwIfNotFound: true);
        m_InWorld_Move = m_InWorld.FindAction("Move", throwIfNotFound: true);
        m_InWorld_Look = m_InWorld.FindAction("Look", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // InWorld
    private readonly InputActionMap m_InWorld;
    private IInWorldActions m_InWorldActionsCallbackInterface;
    private readonly InputAction m_InWorld_Fire1;
    private readonly InputAction m_InWorld_Fire2;
    private readonly InputAction m_InWorld_Move;
    private readonly InputAction m_InWorld_Look;
    public struct InWorldActions
    {
        private @InputControls m_Wrapper;
        public InWorldActions(@InputControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fire1 => m_Wrapper.m_InWorld_Fire1;
        public InputAction @Fire2 => m_Wrapper.m_InWorld_Fire2;
        public InputAction @Move => m_Wrapper.m_InWorld_Move;
        public InputAction @Look => m_Wrapper.m_InWorld_Look;
        public InputActionMap Get() { return m_Wrapper.m_InWorld; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InWorldActions set) { return set.Get(); }
        public void SetCallbacks(IInWorldActions instance)
        {
            if (m_Wrapper.m_InWorldActionsCallbackInterface != null)
            {
                @Fire1.started -= m_Wrapper.m_InWorldActionsCallbackInterface.OnFire1;
                @Fire1.performed -= m_Wrapper.m_InWorldActionsCallbackInterface.OnFire1;
                @Fire1.canceled -= m_Wrapper.m_InWorldActionsCallbackInterface.OnFire1;
                @Fire2.started -= m_Wrapper.m_InWorldActionsCallbackInterface.OnFire2;
                @Fire2.performed -= m_Wrapper.m_InWorldActionsCallbackInterface.OnFire2;
                @Fire2.canceled -= m_Wrapper.m_InWorldActionsCallbackInterface.OnFire2;
                @Move.started -= m_Wrapper.m_InWorldActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_InWorldActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_InWorldActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_InWorldActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_InWorldActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_InWorldActionsCallbackInterface.OnLook;
            }
            m_Wrapper.m_InWorldActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Fire1.started += instance.OnFire1;
                @Fire1.performed += instance.OnFire1;
                @Fire1.canceled += instance.OnFire1;
                @Fire2.started += instance.OnFire2;
                @Fire2.performed += instance.OnFire2;
                @Fire2.canceled += instance.OnFire2;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
            }
        }
    }
    public InWorldActions @InWorld => new InWorldActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    public interface IInWorldActions
    {
        void OnFire1(InputAction.CallbackContext context);
        void OnFire2(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
    }
}
