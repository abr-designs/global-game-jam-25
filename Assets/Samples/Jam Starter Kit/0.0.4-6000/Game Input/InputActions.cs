//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/Samples/Jam Starter Kit/0.0.4-6000/Game Input/InputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace GameInput
{
    public partial class @InputActions: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""4f2bd542-b0e6-4a1f-aac5-1e46a433c334"",
            ""actions"": [
                {
                    ""name"": ""HorizontalMovement"",
                    ""type"": ""Value"",
                    ""id"": ""18d9ff74-8ec0-4ca0-ad0d-b067104cdbee"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""VerticalMovement"",
                    ""type"": ""Button"",
                    ""id"": ""bd12a9d6-db60-4d4d-974b-025888a450f1"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a65766b7-fa05-44b6-8d23-f21fac251c0b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Mouse Left Click"",
                    ""type"": ""Button"",
                    ""id"": ""426bd2d0-af89-422c-9959-a5d5d2ba189d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Mouse Right Click"",
                    ""type"": ""Button"",
                    ""id"": ""0710c818-4bfa-4517-8352-d9ce6c382a0c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseLook"",
                    ""type"": ""Value"",
                    ""id"": ""055413f7-0519-4283-a80e-6c8d35c0f45d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""AD"",
                    ""id"": ""7f3f522a-0e88-414c-836f-be2ea151a320"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b2e33e55-4cb9-4534-92d4-d8a2a05dac1f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""7e78ae0d-3452-4fae-9c24-638bfa104cb5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""e6f03794-d0a2-4b0a-96df-e2299ee5548e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""e8a5d7c7-8d4e-4922-8e0f-96dcd1dac1b7"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c045672d-be59-4760-9bf6-0421165fa638"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WS"",
                    ""id"": ""ffe14137-de7c-4b76-b9d0-f5c3619f2374"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""97e093b9-748b-476d-a8c5-06fb95ee89ab"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""38bdaa30-7803-467c-aa55-6b859f5f41ac"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""5edbf845-9c96-47eb-a3b9-3f591a2ed906"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""1883e6c8-c4bd-438d-b40e-2f1100482fd6"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a0b73c13-927d-44e0-88a6-099a41f919ae"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""074ec06e-9645-434a-9fa4-deeead85e4ea"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7040ecd5-918f-46a9-a9e3-c87cab51b03f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse Left Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0216bcf-adc4-4f7d-8059-1222ca40900b"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse Right Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""61ef18f6-de37-4244-947f-a2f53ede701d"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""1e29f2a9-3106-48c5-a898-8091e2700d90"",
            ""actions"": [],
            ""bindings"": []
        }
    ],
    ""controlSchemes"": []
}");
            // Gameplay
            m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
            m_Gameplay_HorizontalMovement = m_Gameplay.FindAction("HorizontalMovement", throwIfNotFound: true);
            m_Gameplay_VerticalMovement = m_Gameplay.FindAction("VerticalMovement", throwIfNotFound: true);
            m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
            m_Gameplay_MouseLeftClick = m_Gameplay.FindAction("Mouse Left Click", throwIfNotFound: true);
            m_Gameplay_MouseRightClick = m_Gameplay.FindAction("Mouse Right Click", throwIfNotFound: true);
            m_Gameplay_MouseLook = m_Gameplay.FindAction("MouseLook", throwIfNotFound: true);
            // Menu
            m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        }

        ~@InputActions()
        {
            UnityEngine.Debug.Assert(!m_Gameplay.enabled, "This will cause a leak and performance issues, InputActions.Gameplay.Disable() has not been called.");
            UnityEngine.Debug.Assert(!m_Menu.enabled, "This will cause a leak and performance issues, InputActions.Menu.Disable() has not been called.");
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

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Gameplay
        private readonly InputActionMap m_Gameplay;
        private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();
        private readonly InputAction m_Gameplay_HorizontalMovement;
        private readonly InputAction m_Gameplay_VerticalMovement;
        private readonly InputAction m_Gameplay_Jump;
        private readonly InputAction m_Gameplay_MouseLeftClick;
        private readonly InputAction m_Gameplay_MouseRightClick;
        private readonly InputAction m_Gameplay_MouseLook;
        public struct GameplayActions
        {
            private @InputActions m_Wrapper;
            public GameplayActions(@InputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @HorizontalMovement => m_Wrapper.m_Gameplay_HorizontalMovement;
            public InputAction @VerticalMovement => m_Wrapper.m_Gameplay_VerticalMovement;
            public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
            public InputAction @MouseLeftClick => m_Wrapper.m_Gameplay_MouseLeftClick;
            public InputAction @MouseRightClick => m_Wrapper.m_Gameplay_MouseRightClick;
            public InputAction @MouseLook => m_Wrapper.m_Gameplay_MouseLook;
            public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
            public void AddCallbacks(IGameplayActions instance)
            {
                if (instance == null || m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
                @HorizontalMovement.started += instance.OnHorizontalMovement;
                @HorizontalMovement.performed += instance.OnHorizontalMovement;
                @HorizontalMovement.canceled += instance.OnHorizontalMovement;
                @VerticalMovement.started += instance.OnVerticalMovement;
                @VerticalMovement.performed += instance.OnVerticalMovement;
                @VerticalMovement.canceled += instance.OnVerticalMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @MouseLeftClick.started += instance.OnMouseLeftClick;
                @MouseLeftClick.performed += instance.OnMouseLeftClick;
                @MouseLeftClick.canceled += instance.OnMouseLeftClick;
                @MouseRightClick.started += instance.OnMouseRightClick;
                @MouseRightClick.performed += instance.OnMouseRightClick;
                @MouseRightClick.canceled += instance.OnMouseRightClick;
                @MouseLook.started += instance.OnMouseLook;
                @MouseLook.performed += instance.OnMouseLook;
                @MouseLook.canceled += instance.OnMouseLook;
            }

            private void UnregisterCallbacks(IGameplayActions instance)
            {
                @HorizontalMovement.started -= instance.OnHorizontalMovement;
                @HorizontalMovement.performed -= instance.OnHorizontalMovement;
                @HorizontalMovement.canceled -= instance.OnHorizontalMovement;
                @VerticalMovement.started -= instance.OnVerticalMovement;
                @VerticalMovement.performed -= instance.OnVerticalMovement;
                @VerticalMovement.canceled -= instance.OnVerticalMovement;
                @Jump.started -= instance.OnJump;
                @Jump.performed -= instance.OnJump;
                @Jump.canceled -= instance.OnJump;
                @MouseLeftClick.started -= instance.OnMouseLeftClick;
                @MouseLeftClick.performed -= instance.OnMouseLeftClick;
                @MouseLeftClick.canceled -= instance.OnMouseLeftClick;
                @MouseRightClick.started -= instance.OnMouseRightClick;
                @MouseRightClick.performed -= instance.OnMouseRightClick;
                @MouseRightClick.canceled -= instance.OnMouseRightClick;
                @MouseLook.started -= instance.OnMouseLook;
                @MouseLook.performed -= instance.OnMouseLook;
                @MouseLook.canceled -= instance.OnMouseLook;
            }

            public void RemoveCallbacks(IGameplayActions instance)
            {
                if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IGameplayActions instance)
            {
                foreach (var item in m_Wrapper.m_GameplayActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public GameplayActions @Gameplay => new GameplayActions(this);

        // Menu
        private readonly InputActionMap m_Menu;
        private List<IMenuActions> m_MenuActionsCallbackInterfaces = new List<IMenuActions>();
        public struct MenuActions
        {
            private @InputActions m_Wrapper;
            public MenuActions(@InputActions wrapper) { m_Wrapper = wrapper; }
            public InputActionMap Get() { return m_Wrapper.m_Menu; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
            public void AddCallbacks(IMenuActions instance)
            {
                if (instance == null || m_Wrapper.m_MenuActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_MenuActionsCallbackInterfaces.Add(instance);
            }

            private void UnregisterCallbacks(IMenuActions instance)
            {
            }

            public void RemoveCallbacks(IMenuActions instance)
            {
                if (m_Wrapper.m_MenuActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IMenuActions instance)
            {
                foreach (var item in m_Wrapper.m_MenuActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_MenuActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public MenuActions @Menu => new MenuActions(this);
        public interface IGameplayActions
        {
            void OnHorizontalMovement(InputAction.CallbackContext context);
            void OnVerticalMovement(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnMouseLeftClick(InputAction.CallbackContext context);
            void OnMouseRightClick(InputAction.CallbackContext context);
            void OnMouseLook(InputAction.CallbackContext context);
        }
        public interface IMenuActions
        {
        }
    }
}
