import 'package:flutter/material.dart';
import 'package:frontend/entities/capsule.dart';
import 'package:frontend/screens/pages/category_capsules/widgets/capsules/tile/capsule_card.dart';

class CapsuleTile extends StatelessWidget {
  const CapsuleTile({
    super.key,
    required this.capsule,
  });

  final Capsule capsule;

  @override
  Widget build(BuildContext context) {
    return Column(children: [
      CapsuleCard(
        capsule: capsule,
      ),
      const Divider()
    ]);
  }
}
