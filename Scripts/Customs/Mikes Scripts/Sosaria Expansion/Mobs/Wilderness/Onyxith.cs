using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;

namespace Server.Mobiles
{
    [CorpseName("an onyxith corpse")]
    public class Onyxith : BaseCreature
    {
        private DateTime _NextOnyxNova;
        private DateTime _NextCorruptingPresence;

        [Constructable]
        public Onyxith()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Onyxith";
            Body = 0x4; // Gargoyle body
            Hue = 0xB97; // Deep Onyx/Black
            BaseSoundID = 0x482;

            SetStr(350, 450);
            SetDex(80, 120);
            SetInt(120, 160);

            SetHits(600, 800);
            SetStam(100, 120);
            SetMana(200, 250);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   70, 85);
            SetResistance(ResistanceType.Energy,   70, 85);

            SetSkill(SkillName.EvalInt,       90.0, 100.0);
            SetSkill(SkillName.Magery,        90.0, 100.0);
            SetSkill(SkillName.MagicResist,   95.0, 110.0);
            SetSkill(SkillName.Tactics,       85.0,  95.0);
            SetSkill(SkillName.Wrestling,     85.0,  95.0);
            SetSkill(SkillName.DetectHidden,  50.0,  60.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 40;

            PackReg(20);
        }

        public Onyxith(Serial serial)
            : base(serial)
        {
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;
        public override int TreasureMapLevel => 4;

        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;
            base.OnActionCombat();

            if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            // Onyx Nova AOE
            if (DateTime.UtcNow >= _NextOnyxNova)
            {
                var targets = AcquireTargets(combatant, 8);
                if (targets.Count >= 2 || (targets.Count == 1 && InRange(combatant, 3)))
                {
                    OnyxNova(targets);
                    _NextOnyxNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                    return;
                }
            }

            // Corrupting Presence debuff
            if (DateTime.UtcNow >= _NextCorruptingPresence)
            {
                CorruptingPresence();
                _NextCorruptingPresence = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
            }
        }

        private List<Mobile> AcquireTargets(Mobile combatant, int range)
        {
            var targets = new List<Mobile>();
            var eable = Map.GetMobilesInRange(Location, range);

            foreach (Mobile m in eable)
            {
                if (m != this && m.Player && CanBeHarmful(m, false))
                    targets.Add(m);
            }

            eable.Free();

            if (combatant.Player && CanBeHarmful(combatant, false) && !targets.Contains(combatant))
                targets.Insert(0, combatant);

            return targets;
        }

        public void OnyxNova(List<Mobile> targets)
        {
            if (Deleted || Map == null || targets.Count == 0) return;

            Animate(20, 7, 1, true, false, 0);
            PlaySound(0x211);
            Say("*The air around Onyxith shimmers with dark energy!*");

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
            {
                if (Deleted || Map == null) return;

                PlaySound(0x665);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x376A, 9, 32, 0xB97, 0, 5029, 0);

                foreach (var m in targets)
                {
                    if (m != null && !m.Deleted && m.Map == Map && InRange(m, 8) && CanBeHarmful(m))
                    {
                        DoHarmful(m);

                        double dist = GetDistanceToSqrt(m);
                        int raw = Utility.RandomMinMax(30, 50);
                        if (dist > 3)
                            raw = (int)(raw * (1.0 - ((dist - 3) * 0.05)));

                        int avgRes = (m.EnergyResistance + m.PhysicalResistance) / 2;
                        int dmg    = (int)(raw * (1.0 - avgRes / 100.0));
                        dmg = Math.Max(1, dmg);

                        AOS.Damage(m, this, dmg, 25, 0, 0, 0, 75);

                        // (Removed invalid resistance-as-skill debuff)
                        m.SendMessage("You feel a lingering chill from the nova!");
                    }
                }
            });
        }

        public void CorruptingPresence()
        {
            if (Deleted || Map == null) return;

            var eable = Map.GetMobilesInRange(Location, 5);

            foreach (Mobile m in eable)
            {
                if (m != this && m.Player && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    if (Utility.RandomBool())
                    {
                        int drain = Utility.RandomMinMax(10, 20);
                        m.Mana -= drain;
                        m.SendMessage("Your mana is drained by Onyxith's presence!");
                        Effects.SendTargetParticles(
                            m, 0x374A, 10, 15, 0xB97, 0, 9502,
                            EffectLayer.Head, 0);
                    }
                    else
                    {
                        int drain = Utility.RandomMinMax(10, 20);
                        m.Stam -= drain;
                        m.SendMessage("Your stamina is drained by Onyxith's presence!");
                        Effects.SendTargetParticles(
                            m, 0x374A, 10, 15, 0xB97, 0, 9502,
                            EffectLayer.Head, 0);
                    }

                    if (Utility.RandomDouble() < 0.2)
                        new CorruptingDamageTimer(m, this).Start();
                }
            }

            eable.Free();
        }

        private class CorruptingDamageTimer : Timer
        {
            private readonly Mobile _target, _from;
            private int _count;

            public CorruptingDamageTimer(Mobile target, Mobile from)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), 4)
            {
                _target = target;
                _from   = from;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (_target == null || _from == null ||
                    _target.Deleted || _from.Deleted ||
                    _from.Map == null || _target.Map == null ||
                    (!_from.InRange(_target, 10)))
                {
                    Stop();
                    return;
                }

                if (_from.CanBeHarmful(_target))
                {
                    _from.DoHarmful(_target);

                    int dmg = Utility.RandomMinMax(3, 6);
                    int avgRes = (_target.EnergyResistance + _target.PhysicalResistance) / 2;
                    int rd = (int)(dmg * (1.0 - avgRes / 100.0));
                    rd = Math.Max(1, rd);

                    AOS.Damage(_target, _from, rd, 0, 0, 0, 0, 100);
                    _target.SendMessage("You feel corrupted by the dark energy!");
                }

                if (++_count >= 4)
                    Stop();
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.30)
            {
                defender.SendAsciiMessage("Onyxith's claws cloud your vision!");
                
                // Apply temporary skill reductions
                defender.AddSkillMod(new TimedSkillMod(
                    SkillName.Tactics,    true, -10, TimeSpan.FromSeconds(5)));
                defender.AddSkillMod(new TimedSkillMod(
                    SkillName.Wrestling,  true, -10, TimeSpan.FromSeconds(5)));

                Effects.SendTargetParticles(
                    defender, 0x3709, 10, 30, 0xB97, 0, 5052,
                    EffectLayer.Head, 0);
                defender.PlaySound(0x1E0);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems);

            if (Utility.RandomDouble() < 0.01)
            {
                // PackItem(new OnyxShard());
            }
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
