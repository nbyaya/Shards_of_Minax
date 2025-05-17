using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells;              // Required for SpellHelper
using Server.Spells.Seventh;      // Required for PoisonSpell
using Server.Spells.Fourth;       // Required for Poison Field

namespace Server.Mobiles
{
    [CorpseName("a Toxinex corpse")]
    public class Toxinex : BaseCreature
    {
        private DateTime _NextToxicNova;
        private DateTime _NextCorrosiveTouch;
        private DateTime _NextAdaptiveVenom;

        [Constructable]
        public Toxinex()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Toxinex";
            Body = 11;
            BaseSoundID = 1170;
            Hue = 0x8A5;

            SetStr(450, 550);
            SetDex(200, 250);
            SetInt(500, 600);

            SetHits(1200, 1500);
            SetStam(200, 250);
            SetMana(500, 600);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Poison, 90);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100); // Immune
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Wrestling, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Poisoning, 120.0); // Grandmaster Poisoning
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.EvalInt, 110.0, 120.0);

            PackItem(new SpidersSilk(20));
            PackItem(new Nightshade(10));

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 40;

            SetWeaponAbility(WeaponAbility.MortalStrike);
        }

        public Toxinex(Serial serial)
            : base(serial)
        {
        }

        public override bool GivesMLMinorArtifact => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Poison.Lethal;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

        // --- Toxic Nova ---
        public void ToxicNova()
        {
            if (DateTime.UtcNow < _NextToxicNova)
                return;

            Animate(9, 5, 1, true, false, 0);
            Say("Feel the blight!");

            _NextToxicNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            Timer.DelayCall(TimeSpan.FromSeconds(3.0), () =>
            {
                var map = this.Map;
                if (map == null)
                    return;

                // Visual effect at Toxinex's location
                Effects.SendLocationParticles(this, 0x36BD, 20, 10, 0, 0, 5044, 0);

                var targets = new List<Mobile>();
                var eable = map.GetMobilesInRange(this.Location, 5);

                foreach (Mobile m in eable)
                    if (m != this && CanBeHarmful(m) && m.Player)
                        targets.Add(m);

                eable.Free();

                foreach (Mobile m in targets)
                {
                    DoHarmful(m);

                    int damage = Utility.RandomMinMax(50, 80);
                    var poisonToApply = Poison.Deadly;

                    if (Utility.RandomDouble() < 0.3)
                        poisonToApply = Poison.Lethal;
                    if (Utility.RandomDouble() < 0.1)
                        poisonToApply = Poison.Greater;

                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    m.ApplyPoison(this, poisonToApply);
                    m.SendMessage("You are engulfed in a toxic cloud!");
                }
            });
        }

        // --- Corrosive Touch ---
        public void CorrosiveTouch(Mobile target)
        {
            if (DateTime.UtcNow < _NextCorrosiveTouch || !CanBeHarmful(target))
                return;

            if (Utility.RandomDouble() < 0.3)
            {
                _NextCorrosiveTouch = DateTime.UtcNow + TimeSpan.FromSeconds(15.0);

                int physReduction   = Utility.RandomMinMax(10, 20);
                int poisonReduction = Utility.RandomMinMax(15, 25);
                var duration        = TimeSpan.FromSeconds(10.0);

                var physMod   = new ResistanceMod(ResistanceType.Physical, -physReduction);
                var poisonMod = new ResistanceMod(ResistanceType.Poison, -poisonReduction);

                target.AddResistanceMod(physMod);
                target.AddResistanceMod(poisonMod);

                Timer.DelayCall(duration, () =>
                {
                    target.RemoveResistanceMod(physMod);
                    target.RemoveResistanceMod(poisonMod);
                    target.SendMessage("The corrosive effect on your armor fades.");
                });

                target.SendMessage("Toxinex's touch corrodes your defenses!");
                Effects.SendTargetParticles(
                    target,
                    0x374A,    // itemID
                    10,        // speed
                    15,        // duration
                    0x8A5,     // hue
                    0,         // renderMode
                    9502,      // effect
                    EffectLayer.Waist, // layer
                    0          // unknown
                );
            }
        }

        // --- Adaptive Venom ---
        public void AdaptiveVenom()
        {
            if (DateTime.UtcNow < _NextAdaptiveVenom || Hits > HitsMax / 3)
                return;

            _NextAdaptiveVenom = DateTime.UtcNow + TimeSpan.FromMinutes(1.0);
            var duration = TimeSpan.FromSeconds(8.0);

            // Ensure 100% poison resist
            var poisonMod = new ResistanceMod(ResistanceType.Poison, 100 - GetResistance(ResistanceType.Poison));
            AddResistanceMod(poisonMod);

            DamageMin += 5;
            DamageMax += 10;

            Say("My venom adapts!");
            Hue = 0x48A;
            Effects.SendLocationParticles(this, 0x373A, 10, 15, 0x48A, 0, 9502, 0);

            Timer.DelayCall(duration, () =>
            {
                RemoveResistanceMod(poisonMod);
                DamageMin -= 5;
                DamageMax -= 10;
                Hue = 0x8A5;
                SendMessage("The adaptive venom effect fades.");
            });
        }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            if (Combatant is Mobile target)
            {
                if (InRange(target.Location, 5))
                    ToxicNova();

                AdaptiveVenom();
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
            CorrosiveTouch(defender);
        }

        // --- Loot and Drops ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.AosUltraRich, 5);
            AddLoot(LootPack.SuperBoss);    // replaced non‑existent LootChampion
            AddLoot(LootPack.Gems);
            AddLoot(LootPack.Potions);

            PackItem(new SpidersSilk(Utility.RandomMinMax(20, 40)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 20)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));

            // Note: Removed the RarePoisonSac and ToxinexEssence lines
            // because those types were not defined.
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.05)
            {
                switch (Utility.Random(3))
                {
                    case 0: c.DropItem(new HunterLegs()); break;
                    case 1: c.DropItem(new MalekisHonor()); break;
                    case 2: c.DropItem(new DaemonBone(Utility.RandomMinMax(5, 10))); break;
                }
            }

            if (Utility.RandomDouble() < 0.2)
                c.DropItem(new ParrotItem());
        }

        // --- Serialization ---
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version == 0)
            {
                _NextToxicNova      = DateTime.UtcNow;
                _NextCorrosiveTouch = DateTime.UtcNow;
                _NextAdaptiveVenom  = DateTime.UtcNow;
            }
            // version 1 has no additional data to read
        }
    }
}
