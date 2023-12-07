import 'package:flutter/material.dart';
import 'package:frontend/entities/capsule.dart';

import '../widgets.dart';

class CapsuleCard extends StatelessWidget {
  final Capsule capsule;

  const CapsuleCard({super.key, required this.capsule});

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.start,
      children: [
        CapsuleImage(imageUrl: capsule.imageUrl),
        _buildCapsuleLabelAndIntensity(),
        const Spacer(),
        _buildCapsulePriceAndQuantity(),
        const SizedBox(width: 10),
      ],
    );
  }

  Column _buildCapsuleLabelAndIntensity() {
    return Column(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        CapsuleLabel(label: capsule.label),
        CapsuleTitle(name: capsule.name),
        if (capsule.intensity != 0)
          CapsuleIntensity(intensity: capsule.intensity),
      ],
    );
  }

  Row _buildCapsulePriceAndQuantity() {
    return Row(
      children: [
        CapsulePrice(price: capsule.price),
        const SizedBox(width: 10),
        const CapsuleQuantity(
          quantity: 2,
        ),
      ],
    );
  }
}
