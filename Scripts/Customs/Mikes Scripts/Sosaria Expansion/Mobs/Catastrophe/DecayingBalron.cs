using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a decaying balron corpse")]
    public class DecayingBalron : BaseCreature
    {
        private DateTime _NextDecayAura;
        private bool _HasExploded = false; // To ensure the explosion only happens once

        [Constructable]
        public DecayingBalron()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Decaying Balron";
            Body = 40;
            BaseSoundID = 357;
            Hue = 0x47E; // A unique decayed green hue.


            // Enhanced attributes
            SetStr(1200, 1400);
            SetDex(250, 300);
            SetInt(300, 400);

            SetHits(1000, 1200);

            SetDamage(30, 40);

            // Damage types can include more elemental flavors (some decay energy!)
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Energy, 20);
            SetDamageType(ResistanceType.Poison, 20);

            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100); // fully immune to poison
            SetResistance(ResistanceType.Energy, 50, 65);

            // Skills increased to reflect its advanced magical decay powers
            SetSkill(SkillName.Anatomy, 40.0, 60.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 105.0, 125.0);
            SetSkill(SkillName.Meditation, 40.0, 60.0);
            SetSkill(SkillName.MagicResist, 110.0, 130.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 50000;
            Karma = -50000;

            VirtualArmor = 120;

            PackItem(new Longsword());
        }

        public DecayingBalron(Serial serial)
            : base(serial)
        {
        }

        public override bool CanFly { get { return true; } }

        public override bool CanRummageCorpses { get { return true; } }

        public override Poison PoisonImmune { get { return Poison.Deadly; } }

        public override int TreasureMapLevel { get { return 6; } }

        public override int Meat { get { return 1; } }

        // This monster periodically emits a decaying aura that damages nearby enemies.
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            // Check basic validity before attempting the ability.
            if (DateTime.UtcNow < _NextDecayAura || combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            // Emit decay aura: affect all mobiles (except itself) in a radius of 3 tiles.
            List<Mobile> toAffect = new List<Mobile>();
            foreach (Mobile m in this.GetMobilesInRange(3))
            {
                if (m != this && CanBeHarmful(m))
                {
                    toAffect.Add(m);
                }
            }

            // Apply aura effect: damage over time and a chance to reduce attributes.
            foreach (Mobile m in toAffect)
            {
                DoHarmful(m);
                int auraDamage = Utility.RandomMinMax(10, 20);
                AOS.Damage(m, this, auraDamage, 0, 0, 25, 0, 50); // damage split: part poison/energy decay

                // Example of an attribute reduction effect (for MOBILES only)
                if (m is Mobile target)
                {
                    // Drain a small amount of stamina
                    target.Stam -= Utility.RandomMinMax(5, 10);
                    target.SendAsciiMessage("You feel your strength wither away...");
                }
            }

            // Set next aura activation time (every 6-10 seconds)
            _NextDecayAura = DateTime.UtcNow + TimeSpan.FromSeconds(6.0 + (4.0 * Utility.RandomDouble()));

            // Optional: Check for low health to trigger a one-time decay explosion.
            if (!_HasExploded && Hits < (HitsMax * 0.25))
            {
                TriggerDecayExplosion();
                _HasExploded = true;
            }
        }

        // When landing a melee attack, the Balron drains the life essence of the target.
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // Ensure our target is valid and is a Mobile
            if (!(Combatant is Mobile target))
                return;

            // 40% chance to attempt a soul drain on melee attack.
            if (0.4 < Utility.RandomDouble())
                return;

            DoHarmful(defender);

            // Drain portion: steal health, mana, and stamina from the target.
            int drainAmount = Utility.RandomMinMax(20, 35);
            AOS.Damage(defender, this, drainAmount, 0, 0, 50, 0, 50); // half energy/poison decay damage
            defender.SendAsciiMessage("Your life force is being stolen!");

            // Only if the target is a Mobile, adjust mana and stam.
            if (target != null)
            {
                int manaDrain = Utility.RandomMinMax(5, 10);
                int stamDrain = Utility.RandomMinMax(5, 10);

                // Check if target has sufficient resources.
                if (target.Mana >= manaDrain)
                    target.Mana -= manaDrain;
                if (target.Stam >= stamDrain)
                    target.Stam -= stamDrain;
            }

            // Heal self for a portion of damage drained.
            this.Hits += drainAmount / 2;
            this.SendAsciiMessage("The Decaying Balron absorbs your essence!");
        }

        // An optional explosion of decaying energy that triggers once when health is low.
        public void TriggerDecayExplosion()
        {
            // Get all mobiles in a radius of 4 tiles.
            List<Mobile> affected = new List<Mobile>();
            foreach (Mobile m in this.GetMobilesInRange(4))
            {
                if (m != this && CanBeHarmful(m))
                    affected.Add(m);
            }

            // Animate explosion effect.
            this.Animate(AnimationType.Attack, 0);

            // Delay explosion slightly for dramatic effect.
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                foreach (Mobile m in affected)
                {
                    DoHarmful(m);
                    int expDamage = Utility.RandomMinMax(30, 50);
                    AOS.Damage(m, this, expDamage, 0, 0, 50, 0, 50);
                    m.SendAsciiMessage("An explosion of decaying energy engulfs you!");
                }
                this.PlaySound(0x208); // Play an explosion sound effect.
            });
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.MedScrolls, 3);

            // Rare drop chance for a unique decayed artifact.
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                PackItem(new InfernalSummonerRobe());
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(_NextDecayAura);
            writer.Write(_HasExploded);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            _NextDecayAura = reader.ReadDateTime();
            _HasExploded = reader.ReadBool();
        }
    }
}
