using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells;           // for ApplyPoison, etc.
using Server.SkillHandlers;    // for bleed, etc.
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a grothuuk corpse")]
    public class Grothuuk : BaseCreature
    {
        // --- Ability Timers and State ---
        private DateTime _NextGroundPoundTime;
        private bool _IsWindingUpGroundPound;
        private DateTime _NextBrutalSlamTime;
        private bool _EnrageActive;
        private DateTime _EnrageEndTime;
        private int _OriginalHue;

        // --- AOE Ground Pound Settings ---
        private const double GroundPoundCooldown = 25.0;
        private const double GroundPoundWindupDuration = 3.0;
        private const int GroundPoundRadius = 6;
        private const int GroundPoundMinDamage = 50;
        private const int GroundPoundMaxDamage = 70;
        private const int GroundPoundEffect = 0x3709;
        private const int GroundPoundSound = 0x2D3;

        // --- Brutal Slam Settings ---
        private const double BrutalSlamChance = 0.3;
        private const double BrutalSlamCooldown = 10.0;
        private const int BrutalSlamExtraDamageMin = 15;
        private const int BrutalSlamExtraDamageMax = 25;
        private const double BrutalSlamHealingReductionPercent = 0.5;
        private const double BrutalSlamHealingReductionDuration = 15.0;

        // --- Enrage Settings ---
        private const double EnrageHealthThreshold = 0.3;
        private const double EnrageDuration = 30.0;
        private const double EnrageDamageBoostPercent = 0.25;
        private const double EnrageResistanceBoostPercent = 0.15;

        [Constructable]
        public Grothuuk()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Grothuuk";
            Body = 267;
            BaseSoundID = 0x59F;
            Hue = 0x81;
            _OriginalHue = Hue;

            SetStr(600, 750);
            SetDex(100, 150);
            SetInt(80, 120);

            SetHits(800, 1000);
            SetDamage(20, 35);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 35, 45);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 90.0, 105.0);
            SetSkill(SkillName.Tactics, 100.0, 115.0);
            SetSkill(SkillName.Wrestling, 100.0, 115.0);

            Fame = 10000;
            Karma = -10000;
            VirtualArmor = 45;

            _NextGroundPoundTime = DateTime.UtcNow;
            _NextBrutalSlamTime = DateTime.UtcNow;
            _EnrageActive = false;
            _EnrageEndTime = DateTime.MinValue;

            // Loot
            PackItem(new Gold(Utility.RandomMinMax(500, 1000)));
            PackItem(new Bandage(Utility.RandomMinMax(10, 20)));
            PackItem(new Ribs());
            PackItem(Loot.RandomPotion());
            PackItem(Loot.RandomPotion());

            if (Utility.RandomDouble() < 0.1)
                PackItem(new TreasureMap(6, Map));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new ObsidianOre(Utility.RandomMinMax(1, 3)));

            if (Utility.RandomDouble() < 0.005)
                PackItem(new GrothuuksFist());
        }

        public Grothuuk(Serial serial)
            : base(serial)
        {
        }

        public override bool CanRummageCorpses { get { return false; } }
        public override int TreasureMapLevel { get { return 4; } }
        public override int Meat { get { return 4; } }

        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 18) || !CanBeHarmful(combatant) || !InLOS(combatant))
            {
                base.OnActionCombat();
                return;
            }

            // Enrage
            if (!_EnrageActive && (double)Hits / HitsMax <= EnrageHealthThreshold)
            {
                ActivateEnrage();
            }
            else if (_EnrageActive && DateTime.UtcNow >= _EnrageEndTime)
            {
                DeactivateEnrage();
            }

            // Ground Pound
            if (!_IsWindingUpGroundPound && DateTime.UtcNow >= _NextGroundPoundTime)
            {
                bool targetsNearby = false;
                foreach (Mobile m in GetMobilesInRange(GroundPoundRadius))
                {
                    if (m != this && m.Player && CanBeHarmful(m, false))
                    {
                        targetsNearby = true;
                        break;
                    }
                }

                if (targetsNearby)
                {
                    StartGroundPound();
                    return;
                }
            }

            if (!_IsWindingUpGroundPound)
            {
                base.OnActionCombat();
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.UtcNow >= _NextBrutalSlamTime && Utility.RandomDouble() < BrutalSlamChance)
            {
                if (defender != null && !defender.Deleted && defender.Alive && CanBeHarmful(defender, false))
                {
                    DoBrutalSlam(defender);
                    _NextBrutalSlamTime = DateTime.UtcNow + TimeSpan.FromSeconds(BrutalSlamCooldown);
                }
            }
        }

        // Ground Pound
        public void StartGroundPound()
        {
            if (_IsWindingUpGroundPound) return;

            _IsWindingUpGroundPound = true;
            PublicOverheadMessage(Network.MessageType.Regular, Hue, false, "*Grothuuk begins to build power for a massive blow!*");
            PlaySound(BaseSoundID + 1);
            Animate(11, 5, 1, true, false, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(GroundPoundWindupDuration), PerformGroundPound);
        }

        public void PerformGroundPound()
        {
            if (!_IsWindingUpGroundPound) return;

            _IsWindingUpGroundPound = false;
            PublicOverheadMessage(Network.MessageType.Regular, Hue, false, "*Grothuuk slams the ground with immense force!*");
            PlaySound(GroundPoundSound);
            Animate(10, 5, 1, true, false, 0);



            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable nearby = GetMobilesInRange(GroundPoundRadius);

            foreach (Mobile m in nearby)
                if (m != this && m.Alive && CanBeHarmful(m, false))
                    targets.Add(m);

            nearby.Free();

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int rawDamage = Utility.RandomMinMax(GroundPoundMinDamage, GroundPoundMaxDamage);

                int totalRes = (target.PhysicalResistance + target.EnergyResistance) / 2;
                if (_EnrageActive)
                    rawDamage = (int)(rawDamage * (1.0 + EnrageDamageBoostPercent));

                int finalDamage = (int)(rawDamage * (1.0 - (totalRes / 100.0)));
                finalDamage = Math.Max(1, finalDamage);

                AOS.Damage(target, this, finalDamage, 70, 0, 0, 0, 30);

                if (Utility.RandomDouble() < 0.25)
                {
                    target.SendLocalizedMessage(1004014);
                    target.Freeze(TimeSpan.FromSeconds(Utility.RandomMinMax(2, 4)));
                }
            }

            _NextGroundPoundTime = DateTime.UtcNow + TimeSpan.FromSeconds(GroundPoundCooldown);
        }

        // Brutal Slam
        public void DoBrutalSlam(Mobile target)
        {
            DoHarmful(target);

            int rawDamage = Utility.RandomMinMax(BrutalSlamExtraDamageMin, BrutalSlamExtraDamageMax);
            if (_EnrageActive)
                rawDamage = (int)(rawDamage * (1.0 + EnrageDamageBoostPercent));

            AOS.Damage(target, this, rawDamage, 100, 0, 0, 0, 0);
            target.SendAsciiMessage("Grothuuk's brutal slam causes a searing pain!");
            PlaySound(0x308);
            target.SendAsciiMessage("Your ability to heal is reduced!");

            // (Healing reduction debuff would go here if you have a system for it)
        }

        // Enrage
        public void ActivateEnrage()
        {
            _EnrageActive = true;
            _EnrageEndTime = DateTime.UtcNow + TimeSpan.FromSeconds(EnrageDuration);

            PublicOverheadMessage(Network.MessageType.Regular, Hue, false, "*Grothuuk enters a terrifying rage!*");
            PlaySound(0x4F4);

            // Temporarily darken the hue
            Hue = _OriginalHue - 20;

            FixedParticles(0x375A, 10, 15, 5017, EffectLayer.Waist);
        }

        public void DeactivateEnrage()
        {
            _EnrageActive = false;
            PublicOverheadMessage(Network.MessageType.Regular, Hue, false, "*Grothuuk's rage subsides.*");

            // Restore original hue
            Hue = _OriginalHue;
        }

        // Serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
            writer.Write(_NextGroundPoundTime);
            writer.Write(_IsWindingUpGroundPound);
            writer.Write(_NextBrutalSlamTime);
            writer.Write(_EnrageActive);
            writer.Write(_EnrageEndTime);
            // _OriginalHue is derived from constructor Hue
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 1)
            {
                _NextGroundPoundTime = reader.ReadDateTime();
                _IsWindingUpGroundPound = reader.ReadBool();
                _NextBrutalSlamTime = reader.ReadDateTime();
                _EnrageActive = reader.ReadBool();
                _EnrageEndTime = reader.ReadDateTime();
            }
            _OriginalHue = Hue;
        }
    }
}

namespace Server.Items
{
    public class ObsidianOre : Item
    {
        [Constructable]
        public ObsidianOre(int amount) : base(0x19B9)
        {
            Name = "Obsidian Ore";
            Stackable = true;
            Amount = amount;
        }

        public ObsidianOre(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class GrothuuksFist : Fists
    {
        [Constructable]
        public GrothuuksFist() : base()
        {
            Name = "Grothuuk's Brutal Fist";
            Hue = 0x81;
            Weight = 5.0;
            Layer = Layer.OneHanded;
            Attributes.BonusHits = 10;
            Attributes.WeaponDamage = 30;
            WeaponAttributes.HitLeechHits = 20;
        }

        public GrothuuksFist(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
