using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;


namespace Server.Mobiles
{
    [CorpseName("a decayed corpse")]
    public class DecayedMage : BaseCreature
    {
        // Timers for the Decay Mage’s special abilities
        private DateTime _NextManaSiphon;
        private DateTime _NextDecayPulse;
        private bool _HasExploded;

        [Constructable]
        public DecayedMage()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a decayed mage";
            Body = 148;              // same body as SkeletalMage
            BaseSoundID = 451;       // same sound as SkeletalMage
            Hue = 0x47E;             // a unique, sickly green hue

            // Advanced attributes: higher and more volatile than a regular Skeletal Mage
            SetStr(100, 120);
            SetDex(70, 90);
            SetInt(230, 250);

            SetHits(120, 140);
            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 60); // Emphasize necrotic energy

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 25, 35);
            SetResistance(ResistanceType.Cold, 55, 65);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 45, 55);

            SetSkill(SkillName.EvalInt, 80.1, 90.0);
            SetSkill(SkillName.Magery, 80.1, 90.0);
            SetSkill(SkillName.MagicResist, 75.1, 90.0);
            SetSkill(SkillName.Tactics, 65.1, 80.0);
            SetSkill(SkillName.Wrestling, 60.1, 75.0);
            SetSkill(SkillName.Necromancy, 100.0, 110.0);
            SetSkill(SkillName.SpiritSpeak, 95.0, 105.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 50;

            PackReg(5);
            PackNecroReg(3, 15);
            PackItem(new Bone());
        }

        public DecayedMage(Serial serial)
            : base(serial)
        {
        }

        // This monster is immune to bleeding and standard poisons
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Regular; } }
        public override TribeType Tribe { get { return TribeType.Undead; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }

        // Advanced Ability: In combat, the Decayed Mage periodically drains mana and inflicts a decay aura.
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != this.Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            // Mana Siphon: Drain some mana from the combatant and add a portion to self
            if (DateTime.UtcNow >= _NextManaSiphon)
            {
                ManaSiphon(combatant);
                _NextManaSiphon = DateTime.UtcNow + TimeSpan.FromSeconds(8.0 + (4.0 * Utility.RandomDouble()));
            }

            // Decay Aura Pulse: Periodically release a pulse that inflicts decay damage over a small area
            if (DateTime.UtcNow >= _NextDecayPulse)
            {
                DecayPulse();
                _NextDecayPulse = DateTime.UtcNow + TimeSpan.FromSeconds(12.0 + (6.0 * Utility.RandomDouble()));
            }
        }

        // Drain mana from the target and convert a portion to self-healing.
        private void ManaSiphon(Mobile target)
        {
            // Ensure the target is within range and valid
            if (!InRange(target, 10) || !InLOS(target))
                return;

            DoHarmful(target);

            // Use the check: if (target is Mobile) is redundant since target is already a Mobile parameter.
            int drainAmount = Utility.RandomMinMax(10, 20);
            if (target.Mana >= drainAmount)
            {
                target.Mana -= drainAmount;
                this.Hits += drainAmount / 2; // Convert half of drained mana to health
                target.SendAsciiMessage("Your life force is sapped by decay!");
                this.PublicOverheadMessage(MessageType.Regular, 0x482, false, "*Siphons life energy*");
            }
            else
            {
                target.Mana = 0;
            }
        }

        // Pulse a decay aura that damages any hostile mobiles in a small radius.
        private void DecayPulse()
        {
            List<Mobile> targets = new List<Mobile>();

            // Get all mobiles within 3 tiles
            foreach (Mobile m in this.GetMobilesInRange(3))
            {
                if (m == this || !CanBeHarmful(m))
                    continue;

                targets.Add(m);
            }

            // Send a visual effect

            // Apply decay damage over time for each target found
            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                // Damage between 8-15 (decay type as energy damage)
                int decayDamage = Utility.RandomMinMax(8, 15);
                AOS.Damage(m, this, decayDamage, 0, 0, 100, 0, 0);
                m.SendAsciiMessage("A pulse of decayed energy scorches you!");
            }
        }

        // When struck by damage, if health drops below a threshold, release a necrotic burst (only once)
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (!_HasExploded && Hits < (HitsMax * 0.25))
            {
                // Trigger the necrotic burst
                NecroticBurst();
                _HasExploded = true;
            }
        }

        // Necrotic burst: damages and poisons nearby enemies
        private void NecroticBurst()
        {
            // Show a unique visual and sound effect for the burst
            PublicOverheadMessage(MessageType.Regular, 0x482, false, "*The Decayed Mage erupts in a burst of necrotic energy!*");
            PlaySound(0x20E);

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in this.GetMobilesInRange(4))
            {
                if (m == this || !CanBeHarmful(m))
                    continue;
                targets.Add(m);
            }

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                // Direct damage from the burst
                int burstDamage = Utility.RandomMinMax(20, 30);
                AOS.Damage(m, this, burstDamage, 0, 0, 100, 0, 0);
                // Apply a deadly poison effect (ensure to use if (m is Mobile target) before affecting)
                if (m is Mobile target)
                {
                    // Adjust poison level and duration as needed
                    target.ApplyPoison(this, Poison.Deadly);
                    target.SendAsciiMessage("You feel the decay infect your body!");
                }
            }
        }

        public override void GenerateLoot()
        {
            // Loot can be adjusted for an advanced monster: increased loot quality and chance for rare items.
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Potions);
            // 0.5% chance for a rare decayed tome

        }

        public override bool CanRummageCorpses { get { return true; } }
        public override int TreasureMapLevel { get { return 4; } }
        public override int Meat { get { return 1; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(_HasExploded);
            writer.Write(_NextManaSiphon);
            writer.Write(_NextDecayPulse);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            _HasExploded = reader.ReadBool();
            _NextManaSiphon = reader.ReadDateTime();
            _NextDecayPulse = reader.ReadDateTime();
        }
    }
}
