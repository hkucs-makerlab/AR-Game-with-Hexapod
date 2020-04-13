package com.hkucsfyp.controller;

import androidx.appcompat.app.AppCompatActivity;

import android.bluetooth.BluetoothDevice;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.MotionEvent;
import android.widget.Button;
import android.widget.ImageButton;

public class MainActivity extends AppCompatActivity {

    private static final String TAG = "MainActivityLog";

    private ControllerService controllerService;

    static public final int REQUEST_BT_GET_DEVICE = 23;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        controllerService = ControllerService.createInstance(MainActivity.this);

        Button btnScan = findViewById(R.id.btn_scan);
        btnScan.setOnClickListener(v -> {
            Intent intent = new Intent(this, BluetoothDevListActivity.class);
            startActivityForResult(intent, REQUEST_BT_GET_DEVICE);
        });

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
        if (requestCode == ControllerService.ENABLE_BLUETOOTH) {
            if (resultCode == RESULT_OK) {
                Intent intent = new Intent(this, BluetoothDevListActivity.class);
                startActivityForResult(intent, REQUEST_BT_GET_DEVICE);
            }
        }
        if (requestCode == REQUEST_BT_GET_DEVICE) {
            if (resultCode == RESULT_OK) {
                final BluetoothDevice mBluetoothDevice;
                mBluetoothDevice = data.getParcelableExtra(BluetoothDevListActivity.EXTRA_KEY_DEVICE);
                if (mBluetoothDevice == null) {
                    return;
                }
                //
                if (controllerService.getConnected()) {
                    controllerService.disconnectBluetooth();
                }
                controllerService.connectHexapod(mBluetoothDevice);
            }
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
                    controllerService.setDPADLetter(letter);
                    break;
                case MotionEvent.ACTION_UP:
                case MotionEvent.ACTION_CANCEL:
                    controllerService.setDPADLetter(ControllerService.DPAD_LETTER.s);
                    break;
            }
            v.performClick();
            return false;
        });
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        controllerService.onDestroy();
    }
}
