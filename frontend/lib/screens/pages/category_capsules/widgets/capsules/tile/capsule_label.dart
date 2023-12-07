import 'package:flutter/material.dart';

class CapsuleLabel extends StatelessWidget {
  const CapsuleLabel({super.key, required this.label});

  final String label;

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(
        vertical: 2,
        horizontal: 8,
      ),
      color: _determineLabelBackgroundColor(),
      child: Text(
        label,
        style: const TextStyle(
          fontSize: 8,
          fontWeight: FontWeight.w500,
          color: Color(0xFFffffff),
        ),
      ),
    );
  }

  Color _determineLabelBackgroundColor() {
    if (label == 'Limited Editions') {
      return const Color(0xFF24454f);
    }

    return const Color(0xFF9b9b9b);
  }
}
