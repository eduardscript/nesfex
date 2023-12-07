import 'package:flutter/material.dart';

class CapsuleQuantity extends StatelessWidget {
  const CapsuleQuantity({
    super.key,
    required this.quantity,
  });

  final int quantity;

  @override
  Widget build(BuildContext context) {
    return Material(
      color: const Color(0xFF3d8705),
      borderRadius: BorderRadius.circular(3),
      child: InkWell(
        onTap: () {},
        child: Container(
          width: 40,
          height: 40,
          alignment: Alignment.center,
          child: Text(
            quantity.toString(),
            style: const TextStyle(
              color: Colors.white,
              fontSize: 14,
              fontWeight: FontWeight.bold,
            ),
          ),
        ),
      ),
    );
  }
}
