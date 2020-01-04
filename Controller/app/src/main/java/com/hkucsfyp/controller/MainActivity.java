package com.hkucsfyp.controller;

import androidx.appcompat.app.AppCompatActivity;

import android.annotation.SuppressLint;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.util.Log;
import android.view.MotionEvent;
import android.widget.Button;
import android.widget.ImageButton;

public class MainActivity extends AppCompatActivity {

    private static final String TAG = "MainActivityLog";

    private ControllerService controllerService;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        controllerService = ControllerService.createInstance(MainActivity.this);

        Button btnConnect = findViewById(R.id.btn_connect);
        btnConnect.setOnClickListener(v -> controllerService.connectHexapod());

        for (ControllerService.MODE_LETTER letter : ControllerService.MODE_LETTER.values()) {
            for (ControllerService.MODE_NUMBER number : ControllerService.MODE_NUMBER.values()) {
                int resourceId = MainActivity.this.getResources().getIdentifier("button_id_" + letter.getValue() + number.getValue(), "id", MainActivity.this.getPackageName());
                ImageButton button = findViewById(resourceId);
                setModeListener(button, letter, number);
            }
        }

        for (ControllerService.DPAD_LETTER letter : ControllerService.DPAD_LETTER.values()) {
            if (letter != ControllerService.DPAD_LETTER.s) {
                int resourceId = MainActivity.this.getResources().getIdentifier("button_id_" + letter.getValue(), "id", MainActivity.this.getPackageName());
                    ImageButton button = findViewById(resourceId);
                    setDPadListener(button, letter);
            }
        }
    }

    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == ControllerService.ENABLE_BLUETOOTH)
            if (resultCode == RESULT_OK) {
                controllerService.connectHexapod();
            }
    }

    void setModeListener(ImageButton button, ControllerService.MODE_LETTER letter, ControllerService.MODE_NUMBER number) {
        button.setOnClickListener(v -> {
            controllerService.setModeLetter(letter);
            controllerService.setModeNumber(number);
        });
    }

    void setDPadListener(ImageButton button, ControllerService.DPAD_LETTER letter) {
        button.setOnTouchListener((v, event) -> {
            switch (event.getAction()) {
                case MotionEvent.ACTION_DOWN:
                    controllerService.setdPADLetter(letter);
                    break;
                case MotionEvent.ACTION_UP:
                case MotionEvent.ACTION_CANCEL:
                    controllerService.setdPADLetter(ControllerService.DPAD_LETTER.s);
                    break;
            }
            v.performClick();
            return false;
        });
    }
}
