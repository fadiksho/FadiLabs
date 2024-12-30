/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    '../**/*.{razor,html}',
    '../Layout/**/*.{razor,html,js}',
    '../../../Web/Web.Server/Components/**/*.{razor,html,js}',
    '../../../Modules/Blog/Modules.Blog.Components/**/*.{razor,html,js}',
    '../../../Modules/Home/Modules.Home.Components/**/*.{razor,html,js}',
    '../../../Modules/Auth0/Modules.Auth0.Components/**/*.{razor,html,js}',
    '../../../Modules/Auth0/Modules.Auth0.Features.Server/**/*.{razor,html,js}'
  ],
  prefix: 'tw-',
  theme: {
    extend: {
      animation: {
        'buttonAnimation': 'button-pop-custom  var(--animation-btn, 0.25s) ease-out',
      },
      transitionProperty: {
        'margin': 'margin',
        'opacity': 'opacity',
      },
      keyframes: {
        leftToRightLoading: {
          '0%': {
            left: '-100%',
          },
          '100%': {
            left: '100%',
          },
        },
        'button-pop-custom':{
          '0%': {
            transform: 'scale(0.95)',
          },
          '100%': {
            left: 'scale(1)',
          },
        }
      },
    },
  },
  daisyui: {
    base: false,
    darkTheme: "dark",
    themes: [
      {
        dark: {
          ...require("daisyui/src/theming/themes")["dark"],
          "primary": "#8b5cf6",
          "primary-content":"#f5f3ff",
          "success":"#22c55e",
          "success-content": "#f0fdf4",
          "warning": "#eab308",
          "warning-content": "#fefce8",
          "error": "#ef4444",
          "error-content":"#fef2f2",
          "--rounded-box": "0rem", // border radius rounded-box utility class, used in card and other large boxes
          "--rounded-btn": "0rem", // border radius rounded-btn utility class, used in buttons and similar element
          "--rounded-badge": "1.9rem", // border radius rounded-badge utility class, used in badges and similar
          "--animation-btn": "0.25s", // duration of animation when you click on button
          "--animation-input": "0.2s", // duration of animation for inputs like checkbox, toggle, radio, etc
          "--btn-focus-scale": "0.95", // scale transform of button when you focus on it
          "--border-btn": "1px", // border width of buttons
          "--tab-border": "1px", // border width of tabs
          "--tab-radius": "0rem", // border radius of tabs
        },
        light: {
          ...require("daisyui/src/theming/themes")["light"],
          "primary": "#8b5cf6",
          "primary-content": "#f5f3ff",
          "success": "#22c55e",
          "success-content": "#f0fdf4",
          "warning": "#eab308",
          "warning-content": "#fefce8",
          "error": "#ef4444",
          "error-content": "#fef2f2",
          "--rounded-box": "0rem", // border radius rounded-box utility class, used in card and other large boxes
          "--rounded-btn": "0rem", // border radius rounded-btn utility class, used in buttons and similar element
          "--rounded-badge": "1.9rem", // border radius rounded-badge utility class, used in badges and similar
          "--animation-btn": "0.25s", // duration of animation when you click on button
          "--animation-input": "0.2s", // duration of animation for inputs like checkbox, toggle, radio, etc
          "--btn-focus-scale": "0.95", // scale transform of button when you focus on it
          "--border-btn": "1px", // border width of buttons
          "--tab-border": "1px", // border width of tabs
          "--tab-radius": "0rem", // border radius of tabs
        }
      }
    ],
  },
  plugins: [
    require("@tailwindcss/typography"),
    require('daisyui'),
    require('tailwindcss-animated')
  ],
}
