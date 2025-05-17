using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells;          // Required for spell effects like slowing
using Server.Spells.Seventh;  // Example: For slows

namespace Server.Mobiles
{
    [CorpseName("a frozen elemental corpse")]
    public class Aurefrost : BaseCreature
    {
        private DateTime _NextFrostNova;
        private bool _IsChannelingFrostNova;
        private TimeSpan _FrostNovaWindupDuration = TimeSpan.FromSeconds(4.0);
        private DateTime _FrostNovaChannelStartTime;

        private DateTime _NextIcyGrasp;
        private TimeSpan _IcyGraspCooldown = TimeSpan.FromSeconds(15.0);

        [Constructable]
        public Aurefrost()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Aurefrost";
            Body = 163;
            BaseSoundID = 263;
            Hue = 0x481;

            SetStr(500, 600);
            SetDex(150, 170);
            SetInt(200, 250);

            SetHits(800, 950);
            SetMana(400, 500);
            SetStam(150, 180);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Cold,      60);
            SetDamageType(ResistanceType.Energy,    30);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     0,  5);
            SetResistance(ResistanceType.Cold,     80, 90);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   50, 60);

            SetSkill(SkillName.MagicResist, 90.1, 100.0);
            SetSkill(SkillName.Tactics,     90.1, 105.0);
            SetSkill(SkillName.Wrestling,   90.1, 105.0);
            SetSkill(SkillName.Magery,      90.1, 100.0);
            SetSkill(SkillName.EvalInt,     80.1,  95.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 60;

            PackItem(new BlackPearl(5));
            PackItem(new MandrakeRoot(5));
            PackItem(new Garlic(5));
            PackItem(new SulfurousAsh(5));
            PackItem(new FrozenTear());  // fixed: was reagent.FrozenTear()
        }

        public Aurefrost(Serial serial) : base(serial) { }

        public override bool BleedImmune => true;
        public override int  TreasureMapLevel => 4;
        public override int  Meat           => 1;

        public override void OnThink()
        {
            base.OnThink();

            if (_IsChannelingFrostNova && DateTime.UtcNow >= _FrostNovaChannelStartTime + _FrostNovaWindupDuration)
            {
                PerformFrostNova();
                _IsChannelingFrostNova = false;
            }
        }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            var combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 15) || !CanBeHarmful(combatant) || !InLOS(combatant))
            {
                _IsChannelingFrostNova = false;
                return;
            }

            // Try Frost Nova
            if (!_IsChannelingFrostNova && DateTime.UtcNow >= _NextFrostNova && Utility.RandomDouble() < 0.2)
            {
                StartFrostNovaChannel();
                return;
            }

            // Otherwise Icy Grasp
            if (!_IsChannelingFrostNova && DateTime.UtcNow >= _NextIcyGrasp && Utility.RandomDouble() < 0.3)
            {
                IcyGrasp(combatant);
                _NextIcyGrasp = DateTime.UtcNow + _IcyGraspCooldown;
            }
        }

        public void StartFrostNovaChannel()
        {
            if (_IsChannelingFrostNova) return;

            _IsChannelingFrostNova = true;
            _FrostNovaChannelStartTime = DateTime.UtcNow;
            _NextFrostNova = DateTime.UtcNow + _FrostNovaWindupDuration + TimeSpan.FromSeconds(20.0 + 10.0 * Utility.RandomDouble());

            // ** fixed: BattleSound isn’t a valid EffectLayer, use Head (or Waist, etc.) instead **
            FixedParticles(0x376A, 1, 14, 9966, 1150, 3, EffectLayer.Head, 0);
            PlaySound(0x10B);
            Say("*A deep chill begins to gather!*");

            // Optional self-effect during channeling
            FixedParticles(0x3779, 1, 20, 0x481, EffectLayer.Waist);
            // ** removed the 14‑arg MovingParticles call; use FixedParticles or a simpler MovingParticles overload **
        }

        public void PerformFrostNova()
        {
            if (!_IsChannelingFrostNova) return;

            Say("*A burst of freezing energy erupts!*");
            PlaySound(0x22D);

            int radius = 5;
            int damage = Utility.RandomMinMax(30, 45);

            var targets = new List<Mobile>();
            var eable   = Map.GetMobilesInRange(Location, radius);
            foreach (Mobile m in eable)
                if (m != this && CanBeHarmful(m, false))
                    targets.Add(m);
            eable.Free();

            foreach (var m in targets)
            {
                DoHarmful(m);

                int adjustedDamage = damage;
                adjustedDamage = (int)(adjustedDamage * (1.0 - m.ColdResistance / 100.0));
                adjustedDamage = Math.Max(1, adjustedDamage);

                AOS.Damage(m, this, adjustedDamage, 0, 0, 100, 0, 0);

                if (Utility.RandomDouble() < 0.75)
                {
                    m.SendLocalizedMessage(1008112, false, Name);
                    m.PlaySound(0x5C6);
                    SpellHelper.AddStatCurse(this, m, StatType.Dex);
                    m.Delta(MobileDelta.WeaponDamage);
                }

                if (Utility.RandomDouble() < 0.5)
                {
                    int stamDrain = Utility.RandomMinMax(10, 25);
                    m.Stam -= stamDrain;
                    m.SendAsciiMessage("The cold drains your stamina!");
                }

                m.FixedParticles(0x374A, 10, 30, 5052, 0x481, 0, EffectLayer.Waist);
                m.PlaySound(0x5C6);
            }
        }

        public void IcyGrasp(Mobile target)
        {
            if (target == null || target.Deleted || target.Map != Map || !InRange(target, 8) || !CanBeHarmful(target) || !InLOS(target))
                return;

            DoHarmful(target);
            Animate(AnimationType.Spell, 0);
            PlaySound(0x10D);



            Timer.DelayCall(TimeSpan.FromSeconds(1.5), () =>
            {
                if (target is Mobile m)
                {
                    int energyDamage = Utility.RandomMinMax(20, 30);
                    energyDamage = (int)(energyDamage * (1.0 - m.EnergyResistance / 100.0));
                    energyDamage = Math.Max(1, energyDamage);

                    AOS.Damage(m, this, energyDamage, 0, 0, 0, 0, 100);

                    if (Utility.RandomDouble() < 0.4)
                    {
                        m.Freeze(TimeSpan.FromSeconds(3.0));
                        m.SendAsciiMessage("You are gripped by icy tendrils!");
                        m.FixedParticles(0x374A, 10, 30, 5052, 0x481, 0, EffectLayer.Head);
                        m.PlaySound(0x204);
                    }
                    else
                    {
                        m.SendAsciiMessage("An icy grasp chills you to the bone!");
                        m.PlaySound(0x5C6);
                    }
                }
            });
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems);
            // AddLoot(LootPack.Artifacts); // removed: doesn’t exist in ServUO
            AddLoot(LootPack.Potions);


        }

        // serialization omitted for brevity...
		public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // Version history

            // Save custom properties
            writer.Write(_NextFrostNova);
            writer.Write(_IsChannelingFrostNova);
            writer.Write(_FrostNovaChannelStartTime);
            writer.Write(_NextIcyGrasp);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 1)
            {
                 _NextFrostNova = reader.ReadDateTime();
                 _IsChannelingFrostNova = reader.ReadBool();
                 _FrostNovaChannelStartTime = reader.ReadDateTime();
                 _NextIcyGrasp = reader.ReadDateTime();
            }

            // If you add more versions later, handle them here
        }
    }
}

////////////////////////////////////////////
// In Server.Items, add your missing reagent:
namespace Server.Items
{
    public class FrozenTear : Item
    {
        [Constructable]
        public FrozenTear() : base(0xF8F)  // choose an appropriate item ID
        {
            Name   = "frozen tear";
            Hue    = 0x481;
            Weight = 0.1;
        }

        public FrozenTear(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }


}
