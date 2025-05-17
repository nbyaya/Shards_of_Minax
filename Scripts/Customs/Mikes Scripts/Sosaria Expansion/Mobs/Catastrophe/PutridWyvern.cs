using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a putrid wyvern corpse")]
    public class PutridWyvern : BaseCreature
    {
        private DateTime _NextBreath;  // Timer for decaying breath ability

        [Constructable]
        public PutridWyvern()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a Putrid Wyvern";
            this.Body = 62; // Inherits the same body from the basic Wyvern
            this.BaseSoundID = 362; // Same basic sound set as the wyvern
            this.Hue = 0x485; // Unique hue to represent its putrid nature



            // Enhanced stats for advanced power:
            this.SetStr(300, 350);
            this.SetDex(200, 220);
            this.SetInt(80, 120);

            this.SetHits(200, 250);

            this.SetDamage(15, 30);

            // Split damage between Physical and heavy Poison (representing acid decay)
            this.SetDamageType(ResistanceType.Physical, 30);
            this.SetDamageType(ResistanceType.Poison, 70);

            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 25, 35);
            this.SetResistance(ResistanceType.Cold, 25, 35);
            this.SetResistance(ResistanceType.Poison, 95, 100);
            this.SetResistance(ResistanceType.Energy, 30, 40);

            this.SetSkill(SkillName.Poisoning, 80.0, 100.0);
            this.SetSkill(SkillName.MagicResist, 80.0, 100.0);
            this.SetSkill(SkillName.Tactics, 80.0, 100.0);
            this.SetSkill(SkillName.Wrestling, 80.0, 100.0);

            this.Fame = 8000;
            this.Karma = -8000;

            this.VirtualArmor = 50;

            // Advanced monsters might drop higher-tier items.
            this.PackItem(new GreaterPoisonPotion());
        }

        public PutridWyvern(Serial serial) : base(serial)
        {
        }

        // Advanced Ability: Decaying (toxic) Breath.
        // Periodically, in combat, the wyvern releases a toxic breath that deals damage and applies a decay effect.
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            if (DateTime.UtcNow < _NextBreath || combatant == null || combatant.Deleted || 
                combatant.Map != Map || !InRange(combatant, 10) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            DecayBreath(combatant);
            _NextBreath = DateTime.UtcNow + TimeSpan.FromSeconds(8.0 + (Utility.RandomDouble() * 5.0)); // 8-13 seconds cooldown
        }

        private void DecayBreath(Mobile target)
        {
            if (target == null || target.Deleted)
                return;

            // Begin the attack: apply harmful action and play visual effects.
            DoHarmful(target);
            Animate(10, 5, 1, true, false, 0); // Example attack animation
            PlaySound(713); // Attack sound similar to the base wyvern

            // Launch visual particles from the wyvern to the target.

            // Delay the application of the breath’s effects to simulate travel time.
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), delegate()
            {
                if (target != null && !target.Deleted && InLOS(target))
                {
                    // Calculate and apply damage using a combination of physical and poison types.
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(target, this, damage, 30, 0, 0, 70, 0);

                    // If the target is a Mobile, attempt to apply a decay effect.
                    if (target is Mobile mTarget)
                    {
                        mTarget.SendAsciiMessage("Your muscles feel withered as the toxic breath saps your strength!");
                        
                        // Here we simulate the decay effect by applying a high-level poison.
                        // (You might wish to implement a custom DoT effect instead.)
                        Poison decayPoison = Poison.Deadly;

                        mTarget.ApplyPoison(this, decayPoison);
                        
                    }
                }
            });
        }

        // Advanced Ability: Corrosive Blood Splatter.
        // When hit in melee, a 30% chance triggers a spray of acidic blood that damages nearby foes.
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (attacker == null || attacker.Deleted || !InRange(attacker, 2) || !CanBeHarmful(attacker))
                return;

            if (Utility.RandomDouble() <= 0.30)
            {
                CorrosiveBloodSplatter(attacker);
            }
        }

        private void CorrosiveBloodSplatter(Mobile target)
        {
            if (target is Mobile mTarget)
            {
                mTarget.SendAsciiMessage("The Putrid Wyvern's corrosive blood sizzles as it splatters onto you!");
                mTarget.PlaySound(716);
                DoHarmful(mTarget);

                // Apply immediate acid damage to the target.
                int baseDamage = Utility.RandomMinMax(10, 15);
                AOS.Damage(mTarget, this, baseDamage, 0, 0, 0, 100, 0); // All damage as poison (acidic)

                // Also affect other nearby mobiles within 1 tile.
                List<Mobile> nearbyTargets = new List<Mobile>();
                foreach (Mobile m in mTarget.GetMobilesInRange(1))
                {
                    if (m != this && m != mTarget && CanBeHarmful(m))
                        nearbyTargets.Add(m);
                }
                foreach (Mobile m in nearbyTargets)
                {
                    DoHarmful(m);
                    int aoeDamage = Utility.RandomMinMax(5, 10);
                    AOS.Damage(m, this, aoeDamage, 0, 0, 0, 100, 0);
                }
            }
        }

        // OnGaveMeleeAttack override: add an extra chance to cause decay on melee strikes.
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() <= 0.25)
            {
                if (defender is Mobile target)
                {
                    target.SendAsciiMessage("The Putrid Wyvern’s razor-sharp claws leave you seared by decay!");
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 20, 0, 0, 80, 0);
                }
            }
        }

        // Override sound methods using the same base sounds.
        public override int GetAttackSound() { return 713; }
        public override int GetAngerSound() { return 718; }
        public override int GetDeathSound() { return 716; }
        public override int GetHurtSound() { return 721; }
        public override int GetIdleSound() { return 725; }

        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override Poison HitPoison { get { return Poison.Deadly; } }

        // Upgraded properties for treasure, meat, and hides.
        public override int TreasureMapLevel { get { return 3; } }
        public override int Meat { get { return 15; } }
        public override int Hides { get { return 25; } }
        public override HideType HideType { get { return HideType.Horned; } }

        public override bool CanFly { get { return true; } }

        // Generate enhanced loot; a rare drop is available as well.
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);
        }

        public override bool ReacquireOnMovement
        {
            get { return true; }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
