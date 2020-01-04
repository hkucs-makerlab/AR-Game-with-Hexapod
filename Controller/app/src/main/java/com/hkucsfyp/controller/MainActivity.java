package com.hkucsfyp.controller;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.util.Log;
import android.widget.Button;

public class MainActivity extends AppCompatActivity {

    private static final String TAG = "MainActivityLog";
    private ControllerService controllerService;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        controllerService = ControllerService.createInstance(MainActivity.this);

        Button btnConnect = findViewById(R.id.button);
        btnConnect.setOnClickListener(v -> controllerService.initBluetooth());
        Button btnSd = findViewById(R.id.button2);
        btnSd.setOnClickListener(v -> controllerService.sd());
    }

    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == ControllerService.ENABLE_BLUETOOTH)
            if (resultCode == RESULT_OK) {
                controllerService.connectHexapod();
            }
    }
}
